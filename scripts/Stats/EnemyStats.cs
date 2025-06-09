

using UnityEngine;

public class EnemyStats : CharacterStats
{

    private Enemy enemy;
    private ItemDrop myDropSystem;

    [Header("Level details")]
    [SerializeField] private int level = 1;

    [Range(0f, 1f)]
    [SerializeField] private float percentageModifier = .4f;

    protected override void Start()
    {
        ApplyLevelModifiers();

        base.Start();

        enemy = GetComponent<Enemy>();

        myDropSystem = GetComponent<ItemDrop>();
    }

    private void ApplyLevelModifiers()
    {
        //Modify(strength);
        //Modify(agility);
        //Modify(intelligence);
        //Modify(vitality);

        Modify(damage);
        Modify(critChance);
        Modify(critPower);


        Modify(maxHealth);
        Modify(armor);
        Modify(evasion);
        Modify(magicResistance);

        Modify(fireDamage);
        Modify(iceDamage);
        Modify(lightingDamage);

    }

    private void Modify(Stats _stat)
    {
        for (int i = 1; i < level; i++)
        {
            float modifier = _stat.GetValue() * percentageModifier;

            _stat.AddModifier(Mathf.RoundToInt(modifier));
        }
    }

    public override void TakeDamage(int _damage, float _attackDir)
    {
        base.TakeDamage(_damage, _attackDir);

    }

    protected override void Die()
    {
        base.Die();
        enemy.Die();
        Debug.Log("enemy die");
        myDropSystem.GenerateDrop();
    }
}
