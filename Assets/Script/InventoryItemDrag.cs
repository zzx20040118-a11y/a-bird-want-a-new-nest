
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class InventoryItemDrag : MonoBehaviour,
    IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;

    public string itemID;

    [Header("吹风机投放设置")]
    public string targetPanelName = "Panel_03";
    public HairDryerAnimationController hairDryerController;

    private Transform originalParent;
    private Vector2 originalPosition;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();

        canvasGroup = GetComponent<CanvasGroup>();
        if (canvasGroup == null)
        {
            canvasGroup = gameObject.AddComponent<CanvasGroup>();
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;

        canvasGroup.blocksRaycasts = false;

        transform.SetParent(canvas.transform, true);
        transform.SetAsLastSibling();
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null) return;

        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool droppedOnTarget = false;

        if (EventSystem.current != null)
        {
            List<RaycastResult> results =
                new List<RaycastResult>();

            PointerEventData pointerData =
                new PointerEventData(EventSystem.current);

            pointerData.position = eventData.position;

            EventSystem.current.RaycastAll(
                pointerData, results);

            GameObject target = GameObject.Find(targetPanelName);

            foreach (RaycastResult result in results)
            {
                Transform hit = result.gameObject.transform;

                if (hit.name == targetPanelName ||
                    (target != null &&
                     hit.IsChildOf(target.transform)))
                {
                    droppedOnTarget = true;
                    break;
                }
            }
        }

        canvasGroup.blocksRaycasts = true;

        // 吹风机成功投放
        if (droppedOnTarget && itemID == "HairDryer")
        {
            if (hairDryerController != null)
            {
                Debug.Log("吹风机投放成功，开始播放动画！");

                // 启动动画，并隐藏 Panel_03
                hairDryerController.PlayHairDryerAnimation();

                // 隐藏背包中的吹风机
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError(
                    "吹风机动画控制器未指定，请检查 Inspector！");

                // 未指定控制器时，让道具返回原位
                transform.SetParent(originalParent, false);
                rectTransform.anchoredPosition = originalPosition;
            }

            return;
        }

        // 其他道具以及投放失败：返回原位
        transform.SetParent(originalParent, false);
        rectTransform.anchoredPosition = originalPosition;
    }
}
