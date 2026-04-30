using System.Collections.Generic;
using UnityEngine;

public class ItemDictionary : MonoBehaviour
{
    public List<Item> itemPrefabs;
    private Dictionary<int, GameObject> itemDict;

    private void Awake()
    {
        Debug.Log("ItemDictionary Awake");

        itemDict = new Dictionary<int, GameObject>();

        for (int i = 0; i < itemPrefabs.Count; i++)
        {
            if (itemPrefabs[i] != null)
            {
                // ⚠️ sementara biarin, nanti bisa dihapus kalau udah stabil
                itemPrefabs[i].ID = i + 1;

                Debug.Log($"Register Item: {itemPrefabs[i].name} with ID {itemPrefabs[i].ID}");
            }
            else
            {
                Debug.LogWarning($"Item prefab index {i} is NULL");
            }
        }

        foreach (Item item in itemPrefabs)
        {
            if (item != null)
            {
                itemDict[item.ID] = item.gameObject;
            }
        }

        Debug.Log($"ItemDictionary initialized with {itemDict.Count} items");
    }

    public GameObject GetItemPrefab(int itemId)
    {
        if (itemDict == null)
        {
            Debug.LogError("ItemDictionary NOT initialized!");
            return null;
        }

        itemDict.TryGetValue(itemId, out GameObject prefab);

        if (prefab == null)
        {
            Debug.LogWarning($"Item with ID {itemId} NOT FOUND");
        }
        else
        {
            Debug.Log($"Item with ID {itemId} loaded: {prefab.name}");
        }

        return prefab;
    }
}