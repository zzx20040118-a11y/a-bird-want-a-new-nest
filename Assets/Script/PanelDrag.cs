using UnityEngine;
using UnityEngine.EventSystems;

public class PanelDrag : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    private RectTransform rectTransform;
    private RectTransform parentRect;
    private Vector2 pointerOffset;

    void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        parentRect = rectTransform.parent as RectTransform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pointerLocalPosition))
        {
            pointerOffset = rectTransform.anchoredPosition - pointerLocalPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pointerLocalPosition))
        {
            rectTransform.anchoredPosition = pointerLocalPosition + pointerOffset;
        }
    }
}
