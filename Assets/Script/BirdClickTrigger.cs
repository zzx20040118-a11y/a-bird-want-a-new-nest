using UnityEngine;
using UnityEngine.EventSystems;

public class BirdClickTrigger : MonoBehaviour, IPointerClickHandler
{
    [Header("Panel 04 动画控制器")]
    public Panel04AnimationController panel04Controller;

    private bool hasClicked;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (hasClicked) return;

        if (panel04Controller == null)
        {
            Debug.LogError("BirdClickTrigger：尚未指定 Panel04AnimationController！");
            return;
        }

        hasClicked = true;

        panel04Controller.PlayPanel04Animation(
            eventData.position,
            eventData.pressEventCamera
        );
    }
}
