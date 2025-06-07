
using UnityEngine;


public enum EquipmentType
{
    Weapon,
    Armor,
    Anulet,//»¤·û
    Flask//Ò©Æ¿

}

[CreateAssetMenu(fileName = "New Item Data", menuName = "Data/Equipment")]
public class ItemDataEquipment : ItemData
{
    public EquipmentType equipmentType;
}
