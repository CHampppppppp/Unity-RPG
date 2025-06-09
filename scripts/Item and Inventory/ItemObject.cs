
using UnityEngine;

//所有item的父类，代表物品类
public class ItemObject : MonoBehaviour
{
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private ItemData itemData;

    private void SetupVisuals()
    {
        if (itemData == null)
            return;

        GetComponent<SpriteRenderer>().sprite = itemData.icon;
        gameObject.name = "Item object - " + itemData.name;
    }

    private void Update()
    {

    }

    public void SetupItem(ItemData _itemData,Vector2 _velocity)
    {
        itemData = _itemData;

        SetupVisuals();
        Debug.Log("Drop Item " + itemData.itemName);
        rb.velocity = _velocity;
    }


    public void PickupItem()
    {
        Inventory.instance.AddItem(itemData);
        Destroy(gameObject);
    }
}
