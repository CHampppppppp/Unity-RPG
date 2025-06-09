using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//仓库类，

public class Inventory : MonoBehaviour
{
    public static Inventory instance;

    public List<ItemData> startingItems;

    public List<InventoryItem> inventory;
    public Dictionary<ItemData, InventoryItem> inventoryDictionary;

    public List<InventoryItem> equipment;//玩家身上的装备，有多件，所以用list
    public Dictionary<ItemDataEquipment, InventoryItem> equipmentDictionary;

    public List<InventoryItem> stash;
    public Dictionary<ItemData, InventoryItem> stashDictionary;

    [Header("Inventory UI")]
    [SerializeField] private Transform inventorySlotParent;
    [SerializeField] private Transform stashSlotParent;
    [SerializeField] private Transform equipmentSlotParent;
    [SerializeField] private Transform statSlotParent;

    private UI_ItemSlot[] inventoryItemSlot;
    private UI_ItemSlot[] stashItemSlot;
    private UI_ItemSlot_Equipment[] equipmentItemSlot;
    private UI_StatSlot[] statSlot;

    [Header("Items cooldown")]
    private float lastTimeUseFlask;
    private float flaskCooldown;



    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        inventory = new List<InventoryItem>();
        inventoryDictionary = new Dictionary<ItemData, InventoryItem>();

        equipment = new List<InventoryItem>();
        equipmentDictionary = new Dictionary<ItemDataEquipment, InventoryItem>();

        stash = new List<InventoryItem>();
        stashDictionary = new Dictionary<ItemData, InventoryItem>();

        inventoryItemSlot = inventorySlotParent.GetComponentsInChildren<UI_ItemSlot>();
        stashItemSlot = stashSlotParent.GetComponentsInChildren<UI_ItemSlot>();
        equipmentItemSlot = equipmentSlotParent.GetComponentsInChildren<UI_ItemSlot_Equipment>();
        statSlot = statSlotParent.GetComponentsInChildren<UI_StatSlot>();

        AddStartingItems();//初始装备

    }

    void AddStartingItems()
    {
        for (int i = startingItems.Count - 1; i >= 0; i--)
        {
            AddItem(startingItems[i]);
        }
    }

    private void Update()
    {

    }

    public void EquipItem(ItemData _item)
    {
        ItemDataEquipment newEquipment = _item as ItemDataEquipment;
        InventoryItem newItem = new InventoryItem(newEquipment);
        ItemDataEquipment OldEquipment = null;


        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == newEquipment.equipmentType)
            {
                OldEquipment = item.Key;
            }
        }

        if (OldEquipment != null)
        {
            UnEquipItem(OldEquipment);
            AddItem(OldEquipment);//放回slot
        }

        equipment.Add(newItem);
        equipmentDictionary.Add(newEquipment, newItem);
        newEquipment.AddModifiers();//增加效果

        RemoveItem(_item);

    }

    public void UnEquipItem(ItemDataEquipment itemToRemove)
    {
        if (equipmentDictionary.TryGetValue(itemToRemove, out InventoryItem value))
        {
            equipment.Remove(value);
            equipmentDictionary.Remove(itemToRemove);
            itemToRemove.RemoveModifiers(); 
        }
    }

    private void UpdateSlotUI()
    {
        for(int i=0;i< equipmentItemSlot.Length; i++)
        {
            foreach(KeyValuePair<ItemDataEquipment,InventoryItem> item in equipmentDictionary)
            {
                if(item.Key.equipmentType == equipmentItemSlot[i].slotType)
                    equipmentItemSlot[i].UpdateSlot(item.Value);
            }
        }


        for (int i = 0; i < inventoryItemSlot.Length; i++)
        {
            if (inventoryItemSlot[i].item != null )
                inventoryItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < stashItemSlot.Length; i++)
        {
            if (stashItemSlot[i].item != null)
                stashItemSlot[i].CleanUpSlot();
        }

        for (int i = 0; i < inventory.Count; i++)
        {
            inventoryItemSlot[i].UpdateSlot(inventory[i]);
        }
        
        for (int i = 0; i < stash.Count; i++)
        {
            stashItemSlot[i].UpdateSlot(stash[i]);
        }

        for (int i = 0; i < statSlot.Length; i++)
        {
            statSlot[i].UpdateStatValueUI();
        }
    }


    public void AddItem(ItemData _item)
    {
        if (_item.itemType == ItemType.Equipment && CanAddItem())
            AddToInventory(_item);
        else if (_item.itemType == ItemType.Material)
            AddToStash(_item);

        UpdateSlotUI();
    }

    public void RemoveItem(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            RemoveFromInventory(_item, value);
        }

        if (stashDictionary.TryGetValue(_item, out InventoryItem stashValue))
        {
            RemoveFromStash(_item, stashValue);
        }

        UpdateSlotUI();
    }

    public bool CanAddItem()
    {
        if (inventory.Count >= inventoryItemSlot.Length)
        {
            Debug.Log("没有空间");
            return false;
        }
        return true;
    }

    private void AddToStash(ItemData _item)
    {
        if (stashDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            stash.Add(newItem);
            stashDictionary.Add(_item, newItem);
        }
    }

    private void AddToInventory(ItemData _item)
    {
        if (inventoryDictionary.TryGetValue(_item, out InventoryItem value))
        {
            value.AddStack();
        }
        else
        {
            InventoryItem newItem = new InventoryItem(_item);
            inventory.Add(newItem);
            inventoryDictionary.Add(_item, newItem);
        }
    }

    private void RemoveFromStash(ItemData _item, InventoryItem stashValue)
    {
        if (stashValue.stackSize <= 1)
        {
            stash.Remove(stashValue);
            stashDictionary.Remove(_item);
        }
        else
        {
            stashValue.RemoveStack();
        }
    }

    private void RemoveFromInventory(ItemData _item, InventoryItem inventoryValue)
    {
        if (inventoryValue.stackSize <= 1)
        {
            inventory.Remove(inventoryValue);
            inventoryDictionary.Remove(_item);
        }
        else
            inventoryValue.RemoveStack();
    }

    public ItemDataEquipment GetEquipmentItem(EquipmentType _type)
    {
        ItemDataEquipment equipedItem = null;

        foreach (KeyValuePair<ItemDataEquipment, InventoryItem> item in equipmentDictionary)
        {
            if (item.Key.equipmentType == _type)
            {
                equipedItem = item.Key;
                //Debug.Log(item.Key);
            }
        }

        return equipedItem;
    }

    public void UseFlask()
    {
        ItemDataEquipment currentFlask = GetEquipmentItem(EquipmentType.Flask);

        if (currentFlask == null)
            return;

        bool canUseFlask = Time.time > lastTimeUseFlask + flaskCooldown;

        if (canUseFlask)
        {
            flaskCooldown = currentFlask.itemCooldown;
            currentFlask.ExecuteItemEffect();
            lastTimeUseFlask = Time.time;
        }
        else
        {
            Debug.Log("Flask is cooling down man");
        }
    }
}
