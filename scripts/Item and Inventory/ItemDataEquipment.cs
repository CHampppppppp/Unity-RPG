
using UnityEditor.Timeline.Actions;
using UnityEngine;


//װ��Equipment����ϸ�֣���Ϊweapon��armor��anulet��flask
public enum EquipmentType
{
    Weapon,
    Armor,
    Anulet,//����
    Flask//ҩƿ

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;

    public float itemCooldown;

    [SerializeField] private ItemEffect[] effects;

    [Header("Major stats")]
    public int agility; // ����һ������ �� 1%chance
    public int intelligence; //����һ�㷨�� �� 1%ħ��
    public int vitality; // ���� hp
    public int strength; // ����һ���˺� �� 1%����

    [Header("Offensive stats")]
    public int damage;
    public int critChance; //������
    public int critPower; //�����˺�


    [Header("Defensive stats")]
    public int health;
    public int armor;
    public int evasion;
    public int magicResistance;

    [Header("Magic stats")]
    public int fireDamage;
    public int iceDamage;
    public int lightingDamage;

    public void AddModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.AddModifier(strength);
        playerStats.agility.AddModifier(agility);
        playerStats.intelligence.AddModifier(intelligence);
        playerStats.vitality.AddModifier(vitality);

        playerStats.damage.AddModifier(damage);
        playerStats.critChance.AddModifier(critChance);
        playerStats.critPower.AddModifier(critPower);

        playerStats.maxHealth.AddModifier(health);
        playerStats.armor.AddModifier(armor);
        playerStats.evasion.AddModifier(evasion);
        playerStats.magicResistance.AddModifier(magicResistance);

        playerStats.fireDamage.AddModifier(fireDamage);
        playerStats.iceDamage.AddModifier(iceDamage);
        playerStats.lightingDamage.AddModifier(lightingDamage);
    }

    public void RemoveModifiers()
    {
        PlayerStats playerStats = PlayerManager.instance.player.GetComponent<PlayerStats>();

        playerStats.strength.RemoveModifier(strength);
        playerStats.agility.RemoveModifier(agility);
        playerStats.intelligence.RemoveModifier(intelligence);
        playerStats.vitality.RemoveModifier(vitality);

        playerStats.damage.RemoveModifier(damage);
        playerStats.critChance.RemoveModifier(critChance);
        playerStats.critPower.RemoveModifier(critPower);

        playerStats.maxHealth.RemoveModifier(health);
        playerStats.armor.RemoveModifier(armor);
        playerStats.evasion.RemoveModifier(evasion);
        playerStats.magicResistance.RemoveModifier(magicResistance);

        playerStats.fireDamage.RemoveModifier(fireDamage);
        playerStats.iceDamage.RemoveModifier(iceDamage);
        playerStats.lightingDamage.RemoveModifier(lightingDamage);
    }

    public void ExecuteItemEffect()
    {
        foreach(var item in effects)
        {
            item.ExecuteEffect();
        }
    }

    public override string GetDes()
    {
        sb.Length = 0;

        AddItemDes(strength, "Strength");
        AddItemDes(agility, "Agility");
        AddItemDes(intelligence, "Intelligence");
        AddItemDes(vitality, "Vitality");
        AddItemDes(damage, "Damage");
        AddItemDes(critChance, "CritChance");
        AddItemDes(critPower, "CritPower");
        AddItemDes(health, "Health");
        AddItemDes(evasion, "Evasion");
        AddItemDes(armor, "Armor");
        AddItemDes(magicResistance, "MagicResis");
        AddItemDes(fireDamage, "Fire");
        AddItemDes(iceDamage, "Ice");
        AddItemDes(lightingDamage, "lighting");


        return sb.ToString();
    }

    private void AddItemDes(int _value, string _name)
    {
        if (_value != 0)
        {
            if (sb.Length > 0)
                sb.AppendLine();
            
            if(_value > 0)
                sb.Append(_name + ":" + _value);
        }
    }
}
