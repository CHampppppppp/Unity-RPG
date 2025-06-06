using UnityEngine;

[CreateAssetMenu(fileName = "New Item data", menuName = "Data/Item")]
public class ItemData : ScriptableObject
{
    public string itemName;
    public Sprite icon;
}
