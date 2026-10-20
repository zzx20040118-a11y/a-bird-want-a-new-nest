using UnityEngine;
using UnityEngine.UI;

public class BirdAnimationController : MonoBehaviour
{
    [Header("小鸟动画参考面板")]
    public GameObject targetPanel;

    [Header("动画结束后显示的完整静态图片")]
    public Sprite finalSprite;

    [Header("飞入位置偏移（可在 Inspector 中调整）")]
    public Vector2 positionOffset = Vector2.zero;

    private Animator animator;
    private RectTransform birdRect;
    private Image birdImage;
    private bool hasPlayed;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        birdRect = GetComponent<RectTransform>();
        birdImage = GetComponent<Image>();
    }

    public void PlayBirdAnimation()
    {
        if (hasPlayed) return;

        if (targetPanel == null)
        {
            Debug.LogError(
                "BirdAnimationController：尚未指定 Panel_02_Final！");
            return;
        }

        if (finalSprite == null)
        {
            Debug.LogError(
                "BirdAnimationController：尚未指定 Final Sprite！");
            return;
        }

        if (animator == null)
            animator = GetComponent<Animator>();

        if (birdImage == null)
            birdImage = GetComponent<Image>();

        if (birdRect == null)
            birdRect = GetComponent<RectTransform>();

        if (animator == null || birdImage == null || birdRect == null)
        {
            Debug.LogError(
                "BirdAnimationRoot 缺少 Animator、Image 或 RectTransform！");
            return;
        }

        RectTransform targetRect =
            targetPanel.GetComponent<RectTransform>();

        if (targetRect == null || birdRect.parent == null)
        {
            Debug.LogError(
                "目标面板缺少 RectTransform，或 BirdAnimationRoot 没有父物体！");
            return;
        }

        // 将目标面板的位置转换到 BirdAnimationRoot 的父级坐标系
        Vector3 targetLocalPosition =
            birdRect.parent.InverseTransformPoint(targetRect.position);

        // 以目标面板的位置为基准，加上可调偏移量
        birdRect.anchoredPosition =
            new Vector2(targetLocalPosition.x, targetLocalPosition.y)
            + positionOffset;

        // 不覆盖 BirdAnimationRoot 已经调整好的尺寸
        hasPlayed = true;

        targetPanel.SetActive(false);
        gameObject.SetActive(true);

        animator.Play("Bird_FlyIn", 0, 0f);
    }

    // 由 Bird_FlyIn 最后一帧的 Animation Event 调用
    public void OnBirdAnimationFinished()
    {
        if (targetPanel == null || finalSprite == null)
        {
            Debug.LogError(
                "BirdAnimationController：Panel_02_Final 或 Final Sprite 尚未指定！");
            return;
        }

        Image targetImage = targetPanel.GetComponent<Image>();

        if (targetImage == null)
        {
            Debug.LogError(
                "Panel_02_Final 缺少 Image 组件！");
            return;
        }

        // 使用单独导入的完整 PNG 替换旧画面
        targetImage.sprite = finalSprite;

        // 保持最终面板原有的显示比例设置
        targetPanel.SetActive(true);

        // 隐藏小鸟动画物体
        gameObject.SetActive(false);
    }
}
