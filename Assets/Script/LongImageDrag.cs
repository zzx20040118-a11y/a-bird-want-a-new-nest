
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LongImageDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler
{
    private RectTransform imageRect;
    private RectTransform eyeMaskRect;
    private RectTransform panelRect;
    private RectTransform panelParentRect;

    private Image eyeMaskImage;
    private Texture2D maskTexture;
    private Rect spriteRect;

    private Canvas canvas;

    private bool draggingImage;
    private bool draggingPanel;
    private Vector2 pointerOffset;

    private void Awake()
    {
        imageRect = GetComponent<RectTransform>();

        // 长图的父物体是 EyeMask
        eyeMaskRect = imageRect.parent as RectTransform;

        if (eyeMaskRect != null)
        {
            eyeMaskImage = eyeMaskRect.GetComponent<Image>();

            if (eyeMaskImage != null && eyeMaskImage.sprite != null)
            {
                maskTexture = eyeMaskImage.sprite.texture;
                spriteRect = eyeMaskImage.sprite.textureRect;
            }

            // EyeMask 的父物体是 Panel_05
            panelRect = eyeMaskRect.parent as RectTransform;
        }

        // Panel_05 的父物体
        if (panelRect != null)
            panelParentRect = panelRect.parent as RectTransform;

        canvas = GetComponentInParent<Canvas>();
    }

    private bool IsInsideEye(
        Vector2 screenPosition,
        Camera eventCamera)
    {
        if (eyeMaskRect == null ||
            eyeMaskImage == null ||
            eyeMaskImage.sprite == null ||
            maskTexture == null)
        {
            return false;
        }

        if (!RectTransformUtility
            .ScreenPointToLocalPointInRectangle(
                eyeMaskRect,
                screenPosition,
                eventCamera,
                out Vector2 localPoint))
        {
            return false;
        }

        Rect rect = eyeMaskRect.rect;

        if (rect.width <= 0f || rect.height <= 0f)
            return false;

        // 转换为 0~1 的归一化坐标
        float u = (localPoint.x - rect.xMin) / rect.width;
        float v = (localPoint.y - rect.yMin) / rect.height;

        if (u < 0f || u > 1f || v < 0f || v > 1f)
            return false;

        // 转换为 Sprite 在原始纹理中的像素坐标
        int x = Mathf.Clamp(
            Mathf.FloorToInt(u * spriteRect.width),
            0,
            Mathf.Max(0, (int)spriteRect.width - 1)
        );

        int y = Mathf.Clamp(
            Mathf.FloorToInt(v * spriteRect.height),
            0,
            Mathf.Max(0, (int)spriteRect.height - 1)
        );

        x += Mathf.RoundToInt(spriteRect.x);
        y += Mathf.RoundToInt(spriteRect.y);

        // Unity 的图片坐标原点在左下角
        Color pixel = maskTexture.GetPixel(x, y);

        return pixel.a > 0.5f;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        draggingImage = false;
        draggingPanel = false;

        if (eyeMaskRect == null ||
            panelRect == null ||
            panelParentRect == null)
        {
            Debug.LogError(
                "LongImageDrag：请检查 Panel_05、EyeMask 和长图的层级！"
            );
            return;
        }

        bool insideEye = IsInsideEye(
            eventData.pressPosition,
            eventData.pressEventCamera
        );

        if (insideEye)
        {
            // 鸟眼内部：只移动长图
            draggingImage = true;
        }
        else
        {
            // 鸟眼外部：移动整个 Panel_05
            draggingPanel = true;

            if (RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    panelParentRect,
                    eventData.pressPosition,
                    eventData.pressEventCamera,
                    out Vector2 localPosition))
            {
                pointerOffset =
                    panelRect.anchoredPosition - localPosition;
            }
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (draggingImage && canvas != null)
        {
            float maxX = Mathf.Max(
                0f,
                (imageRect.rect.width - eyeMaskRect.rect.width) / 2f
            );

            Vector2 pos = imageRect.anchoredPosition;

            pos.x += eventData.delta.x / canvas.scaleFactor;
            pos.x = Mathf.Clamp(pos.x, -maxX, maxX);

            imageRect.anchoredPosition = pos;
        }
        else if (draggingPanel)
        {
            if (RectTransformUtility
                .ScreenPointToLocalPointInRectangle(
                    panelParentRect,
                    eventData.position,
                    eventData.pressEventCamera,
                    out Vector2 localPosition))
            {
                panelRect.anchoredPosition =
                    localPosition + pointerOffset;
            }
        }
    }
}
