
using UnityEngine;
using System.Collections;

public class Panel2PopupPosition : MonoBehaviour
{
    [Header("弹出窗口设置")]
    public RectTransform panel;
    public Animator panelAnimator;
    public float moveDuration = 0.45f;

    [Header("Panel1 循环动画")]
    public Animator panel1Animator;

    private Coroutine moveCoroutine;
    private bool hasTriggered = false;

    public void ShowFromPosition(Vector2 startPosition)
    {
        // 防止重复点击
        if (hasTriggered)
            return;

        hasTriggered = true;

        // 冻结 Panel1，停在当前动画帧
        if (panel1Animator != null)
        {
            panel1Animator.speed = 0f;
        }
        else
        {
            Debug.LogWarning("Panel1 Animator 尚未绑定！");
        }

        if (moveCoroutine != null)
        {
            StopCoroutine(moveCoroutine);
        }

        panel.anchoredPosition = startPosition;

        if (panelAnimator != null)
        {
            panelAnimator.ResetTrigger("PopIn2");
            panelAnimator.SetTrigger("PopIn2");
        }

        moveCoroutine = StartCoroutine(MoveToCenter(startPosition));
    }

    private IEnumerator MoveToCenter(Vector2 startPosition)
    {
        Vector2 targetPosition = Vector2.zero;
        float elapsed = 0f;

        while (elapsed < moveDuration)
        {
            elapsed += Time.unscaledDeltaTime;

            float t = Mathf.Clamp01(elapsed / moveDuration);
            t = t * t * (3f - 2f * t);

            panel.anchoredPosition =
                Vector2.Lerp(startPosition, targetPosition, t);

            yield return null;
        }

        panel.anchoredPosition = targetPosition;
        moveCoroutine = null;
    }
}
