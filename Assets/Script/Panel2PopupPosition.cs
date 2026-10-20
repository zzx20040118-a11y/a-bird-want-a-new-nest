using UnityEngine;
using System.Collections;

public class Panel2PopupPosition : MonoBehaviour
{
    public RectTransform panel;
    public Animator panelAnimator;
    public float moveDuration = 0.45f;

    private Coroutine moveCoroutine;

    public void ShowFromPosition(Vector2 startPosition)
    {
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
