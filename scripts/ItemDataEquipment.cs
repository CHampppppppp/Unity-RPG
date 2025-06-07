
using UnityEngine;


//装备Equipment的再细分，分为weapon、armor、anulet、flask
public enum EquipmentType
{
    Weapon,
    Armor,
    Anulet,//护符
    Flask//药瓶

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;
}
