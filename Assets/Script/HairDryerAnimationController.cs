using UnityEngine;

public class HairDryerAnimationController : MonoBehaviour
{
    [Header("需要隐藏的面板")]
    public GameObject panel03;

    [Header("动画结束后显示的静态图片")]
    public GameObject panel03Final;

    [Header("吹风机结束后播放的小鸟动画")]
    public BirdAnimationController birdAnimationController;

    [Header("动画位置偏移")]
    public Vector2 positionOffset = Vector2.zero;

    private Animator animator;
    private bool isPlaying;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void PlayHairDryerAnimation()
    {
        if (isPlaying) return;

        if (panel03 == null)
        {
            Debug.LogError("Panel_03 尚未指定！");
            return;
        }

        RectTransform panelRect =
            panel03.GetComponent<RectTransform>();

        RectTransform animationRect =
            GetComponent<RectTransform>();

        if (panelRect != null && animationRect != null)
        {
            animationRect.anchoredPosition =
                panelRect.anchoredPosition + positionOffset;
        }

        panel03.SetActive(false);

        if (panel03Final != null)
            panel03Final.SetActive(false);

        gameObject.SetActive(true);

        if (animator == null)
            animator = GetComponent<Animator>();

        if (animator == null)
        {
            Debug.LogError(
                "HairDryerAnimationRoot 没有 Animator！");
            return;
        }

        isPlaying = true;
        animator.Play("HairDryer_Play", 0, 0f);
    }

    
    public void OnHairDryerAnimationFinished()
    {
        if (panel03Final != null && panel03 != null)
        {
            RectTransform source =
                panel03.GetComponent<RectTransform>();

            RectTransform target =
                panel03Final.GetComponent<RectTransform>();

            if (source != null && target != null)
            {
                target.anchoredPosition = source.anchoredPosition;
                target.sizeDelta = source.sizeDelta;
                target.anchorMin = source.anchorMin;
                target.anchorMax = source.anchorMax;
                target.pivot = source.pivot;
                target.localScale = source.localScale;
                target.localRotation = source.localRotation;
            }

            panel03Final.SetActive(true);
        }

        // 隐藏吹风机动画
        gameObject.SetActive(false);
        isPlaying = false;

        // 自动播放小鸟动画
        if (birdAnimationController != null)
        {
            birdAnimationController.PlayBirdAnimation();
        }
        else
        {
            Debug.LogError(
            "尚未绑定 BirdAnimationController！"
            );
        }
    }


}
