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

    [Header("原有吹风机投放设置")]
    public string targetPanelName = "Panel_03";
    public HairDryerAnimationController hairDryerController;

    [Header("Panel_05 收集物投放设置")]
    public bool canDropOnPanel05 = false;
    public Panel05ItemController panel05ItemController;

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

        if (canvas != null)
        {
            transform.SetParent(canvas.transform, true);
            transform.SetAsLastSibling();
        }
    }

    public void OnDrag(PointerEventData eventData)
    {
        if (canvas == null)
            return;

        rectTransform.anchoredPosition +=
            eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        bool droppedOnPanel03 = false;
        bool droppedOnPanel05Zone = false;

        if (EventSystem.current != null)
        {
            List<RaycastResult> results =
                new List<RaycastResult>();

            PointerEventData pointerData =
                new PointerEventData(EventSystem.current);

            pointerData.position = eventData.position;

            EventSystem.current.RaycastAll(pointerData, results);

            GameObject panel03 = GameObject.Find(targetPanelName);
            GameObject dropZone05 =
                GameObject.Find("Panel05DropZone");

            foreach (RaycastResult result in results)
            {
                Transform hit = result.gameObject.transform;

                // 原有 Panel_03 吹风机投放检测
                if (hit.name == targetPanelName ||
                    (panel03 != null &&
                     hit.IsChildOf(panel03.transform)))
                {
                    droppedOnPanel03 = true;
                }

                // Panel_05：检测是否投放到指定区域
                if (dropZone05 != null &&
                    (hit == dropZone05.transform ||
                     hit.IsChildOf(dropZone05.transform)))
                {
                    droppedOnPanel05Zone = true;
                }
            }
        }

        // 恢复射线检测
        canvasGroup.blocksRaycasts = true;

        // 原有吹风机投放逻辑
        if (droppedOnPanel03 && itemID == "HairDryer")
        {
            if (hairDryerController != null)
            {
                Debug.Log("吹风机投放成功，开始播放动画！");

                hairDryerController.PlayHairDryerAnimation();
                gameObject.SetActive(false);
            }
            else
            {
                Debug.LogError("吹风机动画控制器未指定！");
                ReturnToOriginalPosition();
            }

            return;
        }

        // Panel_05 指定区域投放逻辑
        if (canDropOnPanel05 && droppedOnPanel05Zone)
        {
            if (panel05ItemController != null)
            {
                panel05ItemController.PlaceItem(gameObject);

                // 如果投放成功，控制器会移除背包物品并显示场景物体
                if (InventoryManager.Instance != null &&
                    !InventoryManager.Instance.HasItem(itemID))
                {
                    gameObject.SetActive(false);
                    return;
                }
            }
            else
            {
                Debug.LogError(
                    "Panel05ItemController 未指定！"
                );
            }
        }

        // 投放失败，返回原位
        ReturnToOriginalPosition();
    }

    private void ReturnToOriginalPosition()
    {
        if (originalParent != null)
        {
            transform.SetParent(originalParent, false);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}
