using System.Text;
using UnityEngine;


//Item����𣬷�Ϊ����Material �� װ��Equipment
public enum ItemType
{
    Material,
    Equipment
}


[CreateAssetMenu(fileName = "New Item data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public ItemType itemType;
    public string itemName;
    public Sprite icon;

    [Range(0,100)]
    public int dropChance;

    protected StringBuilder sb = new StringBuilder();

    public virtual string GetDes()
    {
        return "";
    }
}
