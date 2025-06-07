
using System;

[Serializable]
public class InventoryItem 
{
    public ItemData data;
    public int stackSize;
    
    public InventoryItem(ItemData _newItemData)
    {
        data = _newItemData;
        AddStack();//默认数量为1
    }

    public void AddStack() => stackSize++;
    public void RemoveStack() => stackSize--;
}
