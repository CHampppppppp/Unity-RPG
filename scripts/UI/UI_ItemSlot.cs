using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;


public class UI_ItemSlot : MonoBehaviour, IPointerDownHandler,IPointerEnterHandler,IPointerExitHandler
{
    [SerializeField] private Image itemImage;
    [SerializeField] private TextMeshProUGUI itemText;

    public InventoryItem item;
    private UI ui;

    private void Start()
    {
        ui = GetComponentInParent<UI>();
    }

    public void UpdateSlot(InventoryItem _newItem)
    {
        item = _newItem;

        itemImage.color = Color.white;

        if (item != null)
        {
            itemImage.sprite = item.data.icon;

            if (item.stackSize > 1)
            {
                itemText.text = item.stackSize.ToString();
            }
            else
            {
                itemText.text = "";
            }
        }
    }

    public void CleanUpSlot()
    {

        itemImage.color = Color.clear;
        //itemImage = null;

        itemText.text = "";
    }


    public virtual void OnPointerDown(PointerEventData eventData)
    {
        if (eventData == null)
            return;

        if (item == null)
            return;

        if(Input.GetKey(KeyCode.LeftControl))
        {
            Inventory.instance.RemoveItem(item.data);
            return;
        }

        if (item.data.itemType == ItemType.Equipment)
            Inventory.instance.EquipItem(item.data);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemTip.ShowTip(item.data as ItemDataEquipment);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (item == null)
            return;

        ui.itemTip.HideTip();
    }
}
 