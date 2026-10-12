
using UnityEngine;
using UnityEngine.EventSystems;

public class ImageClickArea : MonoBehaviour, IPointerClickHandler
{
    public ImageSceneSwitcher sceneSwitcher;

    public void OnPointerClick(PointerEventData eventData)
    {
        sceneSwitcher.SwitchImage();
    }
}
