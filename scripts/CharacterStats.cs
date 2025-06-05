using UnityEngine;

public class CharacterStats : MonoBehaviour
{
    [Header("Major stats")]
    public Stats agility; // ����һ������ �� 1%chance
    public Stats intelligence; //����һ�㷨�� �� 1%ħ��
    public Stats vitality; // ���� hp
    public Stats strength; // ����һ���˺� �� 1%����

    [Header("Offensive stats")]
    public Stats damage;
    public Stats critChance; //������
    public Stats critPower; //�����˺�


    [Header("Defensive stats")]
    public Stats maxHealth;
    [SerializeField] private int currentHealth;
    public Stats armor;
    public Stats evasion;
    public Stats magicResistance;

    [Header("Magic stats")]
    public Stats fireDamage;
    public Stats iceDamage;
    public Stats lightingDamage;

    public bool isIgnited; //��ȼ�������˺�)
    public bool isChilled; //����(����25%����)
    public bool isShocked; //ѣ�Σ�׼ȷ���½�20%��

    private float ignitedTimer;
    private float chilledTimer;
    private float shockedTimer;

    private float igniteDamageCooldown = .3f;//�ڵ�ȼ�ڼ䣬ÿ��0.3���յ�һ���˺�
    private float igniteDamageTimer;
    private int igniteDamage;




    protected virtual void Start()
    {
        critPower.SetDefaultValue(150);
        currentHealth = maxHealth.GetValue();
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
            currentHealth -= igniteDamage;

            if (currentHealth < 0)
                Die();

            igniteDamageTimer = igniteDamageCooldown;
        }
    }

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
        currentHealth -= _damage;

        if (currentHealth < 0)
            Die();
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
            return;//�����ֵС��0����Ӧ�÷���

        bool canApplyIgnite = _fireDamage > _iceDamage && _fireDamage > _lightingDamage;
        bool canApplyChill = _iceDamage > _fireDamage && _iceDamage > _lightingDamage;
        bool canApplyShock = _lightingDamage > _fireDamage && _lightingDamage > _iceDamage;


        //�����ֵ����ȵģ����ȡһ��
        CheckWhichOneToUse(_targetStats, _fireDamage, _iceDamage, _lightingDamage, ref canApplyIgnite, ref canApplyChill, canApplyShock);

        if (canApplyIgnite)
        {
            _targetStats.SetupIgniteDamage(Mathf.RoundToInt(_fireDamage * .2f));
        }


        _targetStats.ApplyAilments(canApplyIgnite, canApplyChill, canApplyShock);

    }

    public void ApplyAilments(bool _ignite, bool _chill, bool _shock)
    {
        if (isIgnited || isChilled || isShocked)
            return;

        if (_ignite)
        {
            isIgnited = _ignite;
            ignitedTimer = 4;
        }   
        
        if (_chill)
        {
            isChilled = _chill;
            chilledTimer = 4;
        }   
        
        if (_shock)
        {
            isShocked = _shock;
            shockedTimer = 4;
        }   

        isIgnited = _ignite;
        isChilled = _chill;
        isShocked = _shock;
    }
    public void SetupIgniteDamage(int _damage)
    {
        igniteDamage = _damage;
    }

    protected virtual void Die()
    {
        //throw new NotImplementedException();
    }
    private void CheckWhichOneToUse(CharacterStats _targetStats, int _fireDamage, int _iceDamage, int _lightingDamage, ref bool canApplyIgnite, ref bool canApplyChill, bool canApplyShock)
    {
        //���ڿ�������random��С��0.5������whileѭ��ֱ���ҵ�����������
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
                canApplyChill = true;
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
}
