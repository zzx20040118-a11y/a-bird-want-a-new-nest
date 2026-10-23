using UnityEngine;

public class StoryCanvasZoomPan : MonoBehaviour
{
    [Header("缩放和平移目标")]
    public RectTransform target;

    [Header("缩放范围")]
    public float minScale = 0.5f;
    public float maxScale = 3f;

    [Header("滚轮缩放速度")]
    public float zoomSpeed = 0.15f;

    [Header("平移设置")]
    public bool enableMiddleMousePan = true;

    private RectTransform parentRect;
    private Canvas parentCanvas;

    private bool isPanning;
    private Vector2 lastMousePosition;

    private Vector2 initialPosition;
    private Vector3 initialScale;
    private bool initialViewSaved;

    private void Awake()
    {
        if (target == null)
            target = transform as RectTransform;

        if (target == null)
        {
            Debug.LogError("请为 StoryCanvasZoomPan 指定 Target。");
            enabled = false;
            return;
        }

        parentRect = target.parent as RectTransform;
        parentCanvas = target.GetComponentInParent<Canvas>();

        if (parentRect == null || parentCanvas == null)
        {
            Debug.LogError("Target 必须位于 Canvas 下，并有 UI 父物体。");
            enabled = false;
            return;
        }

        SaveInitialView();
    }

    private void SaveInitialView()
    {
        initialPosition = target.anchoredPosition;
        initialScale = target.localScale;
        initialViewSaved = true;
    }

    private void Update()
    {
        if (target == null || parentCanvas == null)
            return;

        HandleZoom();
        HandlePan();
    }

    private void HandleZoom()
    {
        float scroll = Input.mouseScrollDelta.y;

        if (Mathf.Abs(scroll) < 0.001f)
            return;

        if (!RectTransformUtility.ScreenPointToLocalPointInRectangle(
            parentRect,
            Input.mousePosition,
            GetCanvasCamera(),
            out Vector2 mouseLocalPosition))
            return;

        float oldScale = target.localScale.x;
        float newScale = Mathf.Clamp(
            oldScale * (1f + scroll * zoomSpeed),
            minScale,
            maxScale
        );

        if (Mathf.Approximately(oldScale, newScale))
            return;

        float ratio = newScale / oldScale;

        target.anchoredPosition =
            mouseLocalPosition -
            (mouseLocalPosition - target.anchoredPosition) * ratio;

        target.localScale = new Vector3(
            newScale,
            newScale,
            target.localScale.z
        );
    }

    private void HandlePan()
    {
        if (!enableMiddleMousePan)
            return;

        if (Input.GetMouseButtonDown(2))
        {
            isPanning = true;
            lastMousePosition = Input.mousePosition;
        }

        if (Input.GetMouseButton(2) && isPanning)
        {
            Vector2 currentMousePosition = Input.mousePosition;
            Vector2 delta = currentMousePosition - lastMousePosition;

            if (parentCanvas.scaleFactor > 0f)
                target.anchoredPosition += delta / parentCanvas.scaleFactor;

            lastMousePosition = currentMousePosition;
        }

        if (Input.GetMouseButtonUp(2))
            isPanning = false;
    }

    private Camera GetCanvasCamera()
    {
        if (parentCanvas.renderMode == RenderMode.ScreenSpaceOverlay)
            return null;

        return parentCanvas.worldCamera;
    }

    public void ResetView()
    {
        if (target == null || !initialViewSaved)
            return;

        target.anchoredPosition = initialPosition;
        target.localScale = initialScale;
        isPanning = false;
    }
}
