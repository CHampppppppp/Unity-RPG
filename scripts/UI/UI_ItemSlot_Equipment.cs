using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_ItemSlot_Equipment : UI_ItemSlot
{
    public EquipmentType slotType;

    private void OnValidate()
    {
        gameObject.name = "Equipment slot - " + slotType.ToString();
    }

    public override void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
        {
            return;
        }
        if (item == null || item.data == null)
        {
            return;
        }
        Inventory.instance.UnEquipItem(item.data as ItemDataEquipment);
        Inventory.instance.AddItem(item.data as ItemDataEquipment);

        CleanUpSlot();

    }
}
