
using UnityEngine;

public class Panel05PopupController : MonoBehaviour
{
    [Header("弹出位置偏移（UI 坐标）")]
    public Vector2 popupOffset = new Vector2(100f, 0f);

    private Animator animator;
    private RectTransform panelRect;
    private bool hasPlayed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        panelRect = GetComponent<RectTransform>();
    }

    // 由鼠标点击逻辑调用
    public void PlayPanel05Popup(
        Vector2 screenPosition,
        Camera eventCamera)
    {
        if (hasPlayed) return;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (panelRect == null || animator == null)
        {
            Debug.LogError("Panel_05 缺少 RectTransform 或 Animator！");
            return;
        }

        RectTransform parentRect =
            panelRect.parent as RectTransform;

        if (parentRect == null)
        {
            Debug.LogError("Panel_05 的父物体没有 RectTransform！");
            return;
        }

        // 将鼠标点击位置转换成父级 UI 坐标
        if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            screenPosition,
            eventCamera,
            out Vector2 localPosition))
        {
            // 点击位置 + 可调偏移
            panelRect.anchoredPosition =
                localPosition + popupOffset;
        }

        hasPlayed = true;

        // 播放已有的 PopOut 动画
        animator.Play("PopOut", 0, 0f);
    }
}
