using UnityEngine;
using TMPro;

public class ItemUIController : MonoBehaviour
{
    public TextMeshProUGUI itemNameText;
    public TextMeshProUGUI descriptionText;

    public void ShowItem(Item item)
    {
        if (item == null)
        {
            itemNameText.text = "";
            descriptionText.text = "";
            return;
        }

        itemNameText.text = item.itemName;
        descriptionText.text = item.description;

        Debug.Log($"DISPLAYING: {item.itemName}");
    }
}