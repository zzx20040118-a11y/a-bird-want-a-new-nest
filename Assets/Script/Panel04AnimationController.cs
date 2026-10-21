
using UnityEngine;

public class Panel04AnimationController : MonoBehaviour
{
    [Header("动画播放物体")]
    public GameObject animationObject;

    [Header("动画结束后显示的静止画面")]
    public GameObject finalPanel;

    [Header("PopOut 位置偏移（UI 坐标）")]
    public Vector2 popupOffset = new Vector2(100f, 0f);

    private Animator animator;
    private bool hasFinished;

    private RectTransform animationRect;
    private RectTransform finalRect;

    private void Awake()
    {
        animator = GetComponent<Animator>();

        if (animationObject != null)
            animationRect = animationObject.GetComponent<RectTransform>();

        if (finalPanel != null)
            finalRect = finalPanel.GetComponent<RectTransform>();
    }

    // 由鼠标点击逻辑调用
    public void PlayPanel04Animation(
        Vector2 screenPosition,
        Camera eventCamera)
    {
        if (hasFinished) return;

        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError("NestIsMissing 缺少 Animator！");
            return;
        }

        if (finalPanel == null)
        {
            Debug.LogError("尚未指定 Panel_04_Final！");
            return;
        }

        if (animationObject != null)
            animationObject.SetActive(true);
        else
            gameObject.SetActive(true);

        // 将鼠标点击位置转换为父级 UI 的本地坐标
        RectTransform targetRect =
            animationRect != null
                ? animationRect
                : GetComponent<RectTransform>();

        RectTransform parentRect =
            targetRect.parent as RectTransform;

        if (parentRect != null &&
            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                parentRect,
                screenPosition,
                eventCamera,
                out Vector2 localPosition))
        {
            // 在点击位置基础上增加可调偏移
            Vector2 popupPosition = localPosition + popupOffset;

            targetRect.anchoredPosition = popupPosition;

            // 最终静止画面使用相同位置
            if (finalRect != null &&
                finalRect.parent == parentRect)
            {
                finalRect.anchoredPosition = popupPosition;
            }
        }

        finalPanel.SetActive(false);

        animator.Play("Panel04_PopOut", 0, 0f);
    }

    // 由故事动画最后一帧的 Animation Event 调用
    public void OnPanel04AnimationFinished()
    {
        if (hasFinished) return;

        hasFinished = true;

        if (finalPanel != null)
            finalPanel.SetActive(true);

        if (animationObject != null)
            animationObject.SetActive(false);
        else
            gameObject.SetActive(false);
    }
}
