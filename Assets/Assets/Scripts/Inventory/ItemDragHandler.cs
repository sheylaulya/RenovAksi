using UnityEngine;
using UnityEngine.EventSystems;

public class ItemDragHandler : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    Transform originalParent;
    CanvasGroup canvasGroup;

    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        transform.SetParent(transform.root);
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = 0.6f;
    }

    public void OnDrag(PointerEventData eventData)
    {
        transform.position = eventData.position;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        canvasGroup.alpha = 1f;

        Slot dropSLot = eventData.pointerEnter?.GetComponent<Slot>();
        if (dropSLot == null)
        {
            GameObject item = eventData.pointerEnter;
            if (item != null)
            {
                dropSLot = item.GetComponentInParent<Slot>();
            }
        }
        Slot originalSlot = originalParent.GetComponent<Slot>();

        if (dropSLot != null)
        {
            if (dropSLot.currentItem != null)
            {
                dropSLot.currentItem.transform.SetParent(originalSlot.transform);
                originalSlot.currentItem = dropSLot.currentItem;
                dropSLot.currentItem.GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
            }
            else
            {
                originalSlot.currentItem = null;
            }

            // Place dragged item in new slot
            transform.SetParent(dropSLot.transform);
            dropSLot.currentItem = gameObject;
        }
        else
        {
            // Return to original slot
            transform.SetParent(originalParent);
        }
        GetComponent<RectTransform>().anchoredPosition = Vector2.zero;
    }
}