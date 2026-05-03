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

        foreach (var item in itemPrefabs)
        {
            Debug.Log($"REGISTER: {item.name} → ID {item.ID}");
        }

        foreach (Item item in itemPrefabs)
        {
            if (item == null)
            {
                Debug.LogWarning("Item prefab NULL");
                continue;
            }

            if (item.ID == 0)
            {
                Debug.LogError($"Item {item.name} has ID 0! Set ID in Inspector!");
                continue;
            }

            if (itemDict.ContainsKey(item.ID))
            {
                Debug.LogError($"DUPLICATE ID: {item.ID} on {item.name}");
                continue;
            }

            itemDict.Add(item.ID, item.gameObject);

            Debug.Log($"Registered Item: {item.name} with ID {item.ID}");
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