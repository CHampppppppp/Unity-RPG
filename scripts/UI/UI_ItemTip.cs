
using TMPro;
using UnityEngine;

public class UI_ItemTip : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemNameText;
    [SerializeField] private TextMeshProUGUI itemTypeText;
    [SerializeField] private TextMeshProUGUI itemDescription;



    void Start()
    {
            
    }

    public void ShowTip(ItemDataEquipment _item)
    {
        if (_item == null)
            return;

        itemNameText.text = _item.itemName;
        itemTypeText.text = _item.equipmentType.ToString();
        itemDescription.text = _item.GetDes();

        gameObject.SetActive(true);
    }

    public void HideTip() => gameObject.SetActive(false); 
}
