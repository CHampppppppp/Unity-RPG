
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
}
