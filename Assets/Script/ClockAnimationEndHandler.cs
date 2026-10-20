using UnityEngine;

public class ClockAnimationEndHandler : MonoBehaviour
{
    [Header("需要隐藏的物体")]
    public GameObject panel02;
    public GameObject clockAnimationRoot;

    [Header("需要显示的静态图片")]
    public GameObject panel02Final;
    public GameObject panel03;

    
    public void OnClockSolvedAnimationFinished()
    {
        RectTransform original =
            panel02.GetComponent<RectTransform>();

        RectTransform finalPanel =
            panel02Final.GetComponent<RectTransform>();

        RectTransform newPanel =
            panel03.GetComponent<RectTransform>();

        // 记录 Panel_02_Final 与 Panel_03 原本的相对偏移
        Vector2 panel3Offset = new Vector2(224.30272f, -426.0013f);

        // 让最终静态面板继承 Panel_02 的当前位置和大小
        finalPanel.anchorMin = original.anchorMin;
        finalPanel.anchorMax = original.anchorMax;
        finalPanel.pivot = original.pivot;
        finalPanel.anchoredPosition = original.anchoredPosition;
        finalPanel.sizeDelta = original.sizeDelta;
        finalPanel.localRotation = original.localRotation;
        finalPanel.localScale = original.localScale;

        // 根据最终面板的位置计算 Panel_03 的位置
        newPanel.anchoredPosition =
            finalPanel.anchoredPosition + panel3Offset;

        // 显示静态图片
        panel02Final.SetActive(true);
        panel03.SetActive(true);

        // 隐藏旧面板及动画
        panel02.SetActive(false);
    }

}
