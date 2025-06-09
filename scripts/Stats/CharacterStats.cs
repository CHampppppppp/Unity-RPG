using System.Collections;
using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEngine;


public enum StatType
{
    agility, // 增加一点闪避 和 1%chance
    intelligence, //增加一点法伤 和 1%魔抗
    vitality, // 增加 hp
    strength, // 增加一点伤害 和 1%力量

    damage,
    critChance, //暴击率
    critPower, //暴击伤害

    health,
    armor,
    evasion,
    magicResistance,

    fireDamage,
    iceDamage,
    lightingDamage
}

public class CharacterStats : MonoBehaviour
{
    private EntityFX fx;

    [Header("Major stats")]
    public Stats agility; // 增加一点闪避 和 1%chance
    public Stats intelligence; //增加一点法伤 和 1%魔抗
    public Stats vitality; // 增加 hp
    public Stats strength; // 增加一点伤害 和 1%力量

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance; //暴击率
    public Stats critPower; //暴击伤害


    [Header("Defensive stats")]
    public Stats maxHealth;
    public int currentHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic stats")]
    public Stats fireDamage; 
    public Stats iceDamage;
    public Stats lightingDamage;

    public bool isIgnited; //点燃（持续伤害)
    public bool isChilled; //冰冻(降低25%护甲)
    public bool isShocked; //眩晕（准确度下降20%）

    [SerializeField] private float ailmentDuration;
    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;//在点燃期间，每隔0.3秒收到一次伤害
    private float igniteDamageTimer;
    private int igniteDamage;
    [SerializeField] private GameObject shockStrikePrefab;
    private int shockDamage;

    protected bool isDead;

    public System.Action OnHealthChanged;


    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = GetMaxHealthValue();

        fx = GetComponent<EntityFX>();
    }

    protected virtual void Update()
    {
        chilledTimer -= Time.deltaTime;
        shockedTimer -= Time.deltaTime;
        ignitedTimer -= Time.deltaTime;

        igniteDamageTimer -= Time.deltaTime;

        if(ignitedTimer < 0)
            isIgnited = false;

        if(chilledTimer < 0)
            isChilled = false;

        if(shockedTimer < 0)
            isShocked = false;

        if(igniteDamageTimer < 0 && isIgnited)
        {
            DecreaseHealthBy(igniteDamage);

            if (currentHealth < 0 && !isDead)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

    public virtual void IncreaseStatBy(int _modifier,float _duration,Stats _statToModify)
    {
        StartCoroutine(StatModCoroutine(_modifier, _duration, _statToModify));
    }

    private IEnumerator StatModCoroutine(int _modifier,float _duration,Stats _statToModify)
    {
        _statToModify.AddModifier(_modifier);

        yield return new WaitForSeconds(_duration);

        _statToModify.RemoveModifier(_modifier);
    }

    #region Damage region
    public virtual void DoDamage(CharacterStats _targetStats,float _attackDir)
    {
        if (TargetCanAvoidAttack(_targetStats))
            return;

        int totalDamage = damage.GetValue() + strength.GetValue();

        if (CanCrit())
        { 
            totalDamage = CalculateCritDamage(totalDamage);   
        }

        totalDamage = CheckTargetArmor(_targetStats, totalDamage);
        _targetStats.TakeDamage(totalDamage, _attackDir);
    }


    public virtual void TakeDamage(int _damage,float _attackDir)
    {
        DecreaseHealthBy(_damage);

        GetComponent<Entity>().DamageImpact(_attackDir);
        fx.StartCoroutine("FlashFX");


        if (currentHealth < 0 && !isDead)
            Die();
    }

    protected virtual void DecreaseHealthBy(int _damage)
    {
        currentHealth -= _damage;

        if(OnHealthChanged != null)
            OnHealthChanged();
    }

    public virtual void IncreaseHealthBy(int _amount)
    {
        currentHealth += _amount;

        if(currentHealth > GetMaxHealthValue())
            currentHealth = GetMaxHealthValue();

        if(OnHealthChanged!=null)
            OnHealthChanged();
    }

    public virtual void DoMagicDamage(CharacterStats _targetStats, float _attackDir)
    {
        int _fireDamage = fireDamage.GetValue();
        int _iceDamage = iceDamage.GetValue();
        int _lightingDamage = lightingDamage.GetValue();

        int totalMagicDamage = _fireDamage + _iceDamage + _lightingDamage + intelligence.GetValue();

        totalMagicDamage = CheckTargetMagicResistance(_targetStats, totalMagicDamage);
        _targetStats.TakeDamage(totalMagicDamage, _attackDir);

        if (Mathf.Max(_fireDamage, _iceDamage, _lightingDamage) <= 0)
            return;//如果数值小于0，不应用法伤

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


        //如果数值有相等的，随机取一个
        CheckWhichOneToUse(_targetStats, _fireDamage, _iceDamage, _lightingDamage, ref canApplyIgnite, ref canApplyChill, canApplyShock);

        if (canApplyIgnite)
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));

        //if(canApplyChill)

        if (canApplyShock)
            _targetStats.SetupShockStrikeDamage(Mathf.RoundToInt(_lightingDamage * .1f));

        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        bool canApply = !isIgnited && !isChilled && !isShocked;

        if (_ignite && canApply)
        {
            isIgnited = _ignite;
            ignitedTimer = ailmentDuration;

            fx.IgniteFxFor(ailmentDuration);
        }   
        
        if (_chill && canApply)
        {
            isChilled = _chill;
            chilledTimer = ailmentDuration;

            float slowPercentge = .3f;
            GetComponent<Entity>().SlowEntityBy(slowPercentge, ailmentDuration);

            fx.ChillFxFor(ailmentDuration);
        }

        if (_shock && canApply)
        {
            if (!isShocked)
            {
                ApplyShock(_shock);

            }
            if (isShocked)
            {
                if (GetComponent<Player>() != null)
                    return;

                HitNearestTargetWithShockStrike();

            }
        }
    }

    public void ApplyShock(bool _shock)
    {
        if (isShocked)
            return;

        isShocked = _shock;
        shockedTimer = ailmentDuration;

        fx.ShockFxFor(ailmentDuration);
    }

    private void HitNearestTargetWithShockStrike()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, 25);

        float closestDistance = Mathf.Infinity;
        Transform closestEnemy = null;

        foreach (var hit in colliders)
        {
            if (hit.GetComponent<Enemy>() != null && Vector2.Distance(transform.position, hit.transform.position) > 1)
            {
                float distanceToEnemy = Vector2.Distance(transform.position, hit.transform.position);

                if (distanceToEnemy < closestDistance)
                {
                    closestDistance = distanceToEnemy;
                    closestEnemy = hit.transform;
                }
            }

            if (closestEnemy == null)
                closestEnemy = transform;

        }

        if (closestEnemy != null)
        {
            GameObject newShockStrike = Instantiate(shockStrikePrefab, transform.position, Quaternion.identity);

            newShockStrike.GetComponent<ShockStrikeController>().SetUp(shockDamage, closestEnemy.GetComponent<CharacterStats>());
        }
    }

    public void SetupIgniteDamage(int _damage) => igniteDamage = _damage;
    public void SetupShockStrikeDamage(int _damage) => shockDamage = _damage;

    protected virtual void Die()
    {
        isDead = true;
    }
    private void CheckWhichOneToUse(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage, ref bool canApplyIgnite, ref bool canApplyChill, bool canApplyShock)
    {
        //由于可能三个random都小于0.5，外套while循环直到找到符合条件的
        while (!canApplyIgnite && !canApplyChill && !canApplyShock)
        {
            if (Random.value < .3f && _fireDamage > 0)
            {
                canApplyIgnite = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            else if (Random.value < .6f && _iceDamage > 0)
            {
                canApplyChill = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;
            }

            else if (Random.value < .9f && _lightingDamage > 0)
            {
                canApplyShock = true;
                _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);
                return;

            }
        }
    }

    private static int CheckTargetMagicResistance(CharacterStats _targetStats, int totalMagicDamage)
    {
        totalMagicDamage -= _targetStats.magicResistance.GetValue() + (_targetStats.intelligence.GetValue() * 3);
        totalMagicDamage = Mathf.Clamp(totalMagicDamage, 0, int.MaxValue);
        return totalMagicDamage;
    }



    private bool TargetCanAvoidAttack(CharacterStats _targetStats)
    {
        int totalEvasion = _targetStats.evasion.GetValue() + _targetStats.agility.GetValue();

        if (isShocked)
            totalEvasion += 20;

        if (Random.Range(0, 100) < totalEvasion)
        {
            return true;
        }
        return false;
    }
    private int CheckTargetArmor(CharacterStats _targetStats, int totalDamage)
    {
        if(_targetStats.isChilled)
            totalDamage -= Mathf.RoundToInt(_targetStats.armor.GetValue() *  .8f);
        else
            totalDamage -= _targetStats.armor.GetValue();
 
        totalDamage = Mathf.Clamp(totalDamage, 0, int.MaxValue);
        return totalDamage;
    }

    private bool CanCrit()
    {
        int totalCritChance = critChance.GetValue() + agility.GetValue();

        if(Random.Range(0, 100) <= totalCritChance)
            return true;

        return false;
    }

    private int CalculateCritDamage(int _damage)
    {
        float totalCritPower = (critPower.GetValue() + strength.GetValue()) *.01f;

        float totalCritDamage = _damage * totalCritPower;

        return Mathf.RoundToInt(totalCritDamage);
              
    }

    #endregion

    public int GetMaxHealthValue()
    {
        return maxHealth.GetValue() + vitality.GetValue() * 5;
    }

    public Stats GetStatsByType(StatType _statType)
    {
        if (_statType == StatType.strength)
            return strength;
        else if (_statType == StatType.agility)
            return agility;
        else if (_statType == StatType.intelligence)
            return intelligence;
        else if (_statType == StatType.vitality)
            return vitality;
        else if (_statType == StatType.damage)
            return damage;
        else if (_statType == StatType.critChance)
            return critChance;
        else if (_statType == StatType.critPower)
            return critPower;
        else if (_statType == StatType.health)
            return maxHealth;
        else if (_statType == StatType.armor)
            return armor;
        else if (_statType == StatType.evasion)
            return evasion;
        else if (_statType == StatType.magicResistance)
            return magicResistance;
        else if (_statType == StatType.fireDamage)
            return fireDamage;
        else if (_statType == StatType.iceDamage)
            return iceDamage;
        else if (_statType == StatType.lightingDamage)
            return lightingDamage;

        return null;
    }
}
