using System.Collections.Generic;
using UnityEngine;

public class inventoryController : MonoBehaviour
{
    private ItemDictionary itemDictionary;
    public GameObject inventoryPanel;
    public GameObject slotPrefab;
    public int slotCount;

    void Start()
    {
        Debug.Log("inventoryController Start");

        itemDictionary = FindAnyObjectByType<ItemDictionary>();

        if (itemDictionary == null)
        {
            Debug.LogError("ItemDictionary NOT FOUND in scene!");
        }
    }

    public List<InventorySaveData> GetInventoryItems()
    {
        Debug.Log("=== SAVING INVENTORY ===");

        List<InventorySaveData> invData = new List<InventorySaveData>();

        foreach (Transform slotTransform in inventoryPanel.transform)
        {
            Slot slot = slotTransform.GetComponent<Slot>();

            if (slot == null)
            {
                continue;
            }

            Debug.Log($"Checking slot {slotTransform.GetSiblingIndex()}");

            if (slot.currentItem == null && slotTransform.childCount > 0)
            {
                slot.currentItem = slotTransform.GetChild(0).gameObject;
                Debug.LogWarning("Recovered currentItem from child");
            }

            if (slot.currentItem != null)
            {
                Item item = slot.currentItem.GetComponent<Item>();

                if (item == null)
                {
                    Debug.LogError("Item component missing!");
                    continue;
                }

                Debug.Log($"Saving item ID {item.ID}");

                invData.Add(new InventorySaveData
                {
                    itemID = item.ID,
                    slotIndex = slotTransform.GetSiblingIndex()
                });
            }
        }

        Debug.Log($"TOTAL ITEMS SAVED: {invData.Count}");
        return invData;
    }

    public void SetInventoryItems(List<InventorySaveData> invData)
    {
        Debug.Log("=== LOADING INVENTORY ===");


        if (invData == null)
        {
            Debug.LogWarning("Inventory data is NULL");
            return;
        }

        Debug.Log($"Items to load: {invData.Count}");

        foreach (Transform child in inventoryPanel.transform)
        {
            Destroy(child.gameObject);
        }

        // Recreate slots
        for (int i = 0; i < slotCount; i++)
        {
            Instantiate(slotPrefab, inventoryPanel.transform);
        }

        foreach (InventorySaveData data in invData)
        {
            Debug.Log($"Loading item ID {data.itemID} into slot {data.slotIndex}");

            if (data.slotIndex >= slotCount)
            {
                Debug.LogWarning($"Invalid slot index: {data.slotIndex}");
                continue;
            }

            Slot slot = inventoryPanel.transform.GetChild(data.slotIndex).GetComponent<Slot>();

            GameObject itemPrefab = itemDictionary.GetItemPrefab(data.itemID);

            if (itemPrefab == null)
            {
                Debug.LogError($"Prefab for item ID {data.itemID} NOT FOUND");
                continue;
            }

            GameObject newItem = Instantiate(itemPrefab, slot.transform);
            newItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;

            slot.currentItem = newItem;

            Debug.Log($"Item {newItem.name} LOADED into slot {data.slotIndex}");
        }
    }
}