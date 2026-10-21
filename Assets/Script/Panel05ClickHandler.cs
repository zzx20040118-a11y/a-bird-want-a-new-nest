using UnityEngine;
using UnityEngine.EventSystems;

public class Panel05ClickHandler : MonoBehaviour, IPointerClickHandler
{
    [Header("需要弹出的面板")]
    public GameObject panel05;

    [Header("Panel_05 弹出控制器")]
    public Panel05PopupController popupController;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (panel05 == null || popupController == null)
        {
            Debug.LogError("Panel05ClickHandler：请指定 Panel_05 和 Popup Controller！");
            return;
        }

        // 启用面板，让它可以播放动画
        panel05.SetActive(true);

        // 传入真实鼠标点击位置和 UI 相机
        popupController.PlayPanel05Popup(
            eventData.position,
            eventData.pressEventCamera
        );
    }
}
