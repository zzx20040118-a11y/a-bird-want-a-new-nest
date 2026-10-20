using UnityEngine;
using UnityEngine.EventSystems;

public class ClockHandRotate : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform handRect;
    private RectTransform parentRect;

    private void Awake()
    {
        handRect = GetComponent<RectTransform>();
        parentRect = handRect.parent as RectTransform;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RotateToMouse(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RotateToMouse(eventData);
    }

    private void RotateToMouse(PointerEventData eventData)
    {
        if (parentRect == null)
            return;

        Vector2 localPoint;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            eventData.pressEventCamera,
            out localPoint))
        {
            Vector2 direction = localPoint - handRect.anchoredPosition;

            float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;

            handRect.localRotation = Quaternion.Euler(0f, 0f, angle - 90f);
        }
    }
}