
using UnityEngine;
using UnityEngine.EventSystems;

public class PointerDrag : MonoBehaviour, IPointerDownHandler, IDragHandler
{
    private RectTransform pointer;
    private RectTransform parentRect;
    private Canvas canvas;

    public void Awake()
    {
        pointer = GetComponent<RectTransform>();
        parentRect = pointer.parent as RectTransform;
        canvas = GetComponentInParent<Canvas>();
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        RotatePointer(eventData);
    }

    public void OnDrag(PointerEventData eventData)
    {
        RotatePointer(eventData);
    }

    private void RotatePointer(PointerEventData eventData)
    {
        if (parentRect == null) return;

        Camera cam = null;

        if (canvas != null &&
            canvas.renderMode != RenderMode.ScreenSpaceOverlay)
        {
            cam = eventData.pressEventCamera;
        }

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            eventData.position,
            cam,
            out Vector2 mousePosition))
        {
            Vector2 pivotPosition = pointer.localPosition;
            Vector2 direction = mousePosition - pivotPosition;

            if (direction.sqrMagnitude < 0.001f) return;

            float angle = Mathf.Atan2(direction.y, direction.x)
                          * Mathf.Rad2Deg;

            pointer.localRotation = Quaternion.Euler(0, 0, angle - 90f);
        }
    }
}
