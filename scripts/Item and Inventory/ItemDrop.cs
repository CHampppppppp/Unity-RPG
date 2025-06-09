using System;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;


public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int dropAmount;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject dropPrefab;

    public void GenerateDrop()
    {
        Debug.Log("generate drop");

        for (int i = 0; i < possibleDrop.Length; i++)
        {
            if(Random.Range(0,100) <= possibleDrop[i].dropChance)
                dropList.Add(possibleDrop[i]);
        }

        //if (dropList.Count < 1)
        //    return;

        for (int i = 0; i < dropAmount && dropList.Count > 1; i++)
        {
            ItemData randomItem = dropList[Random.Range(0,dropList.Count - 1)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }
    }


    public void DropItem(ItemData _itemData)
    {
        GameObject newDrop = Instantiate(dropPrefab,transform.position,Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-2, 2), Random.Range(5, 10));


        newDrop.GetComponent<ItemObject>().SetupItem(_itemData, randomVelocity); 
    }
}
