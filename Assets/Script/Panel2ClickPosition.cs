using UnityEngine;
using UnityEngine.EventSystems;

public class Panel2ClickPosition : MonoBehaviour, IPointerClickHandler
{
    public Panel2PopupPosition popupPosition;

    public void OnPointerClick(PointerEventData eventData)
    {
        RectTransform canvasRect =
            popupPosition.GetComponent<RectTransform>().parent as RectTransform;

        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            canvasRect,
            eventData.position,
            eventData.pressEventCamera,
            out Vector2 localPosition))
        {
            popupPosition.ShowFromPosition(localPosition);
        }
    }
}
