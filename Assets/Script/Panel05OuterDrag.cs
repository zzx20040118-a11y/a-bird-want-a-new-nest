using UnityEngine;
using UnityEngine.EventSystems;

public class Panel05OuterDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler
{
    private RectTransform panelRect;
    private RectTransform parentRect;
    private Vector2 pointerOffset;

    private void Awake()
    {
        // 这个脚本挂在透明区域上，但移动的是它的父物体 Panel_05
        panelRect = transform.parent as RectTransform;

        if (panelRect != null)
            parentRect = panelRect.parent as RectTransform;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (panelRect == null || parentRect == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.pressPosition,
            eventData.pressEventCamera,
            out Vector2 pointerLocalPosition))
        {
            pointerOffset =
                panelRect.anchoredPosition - pointerLocalPosition;
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (panelRect == null || parentRect == null)
            return;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 pointerLocalPosition))
        {
            panelRect.anchoredPosition =
                pointerLocalPosition + pointerOffset;
        }
    }
}
