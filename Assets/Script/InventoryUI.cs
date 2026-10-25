
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemList;
    public GameObject itemCardPrefab;

    [Header("背包格子尺寸")]
    public float cardWidth = 100f;
    public float cardHeight = 100f;

    [Header("吹风机动画控制器")]
    public HairDryerAnimationController hairDryerController;

    [Header("Panel_05 投放设置")]
    public Panel05ItemController panel05ItemController;

    [Header("允许投放到 Panel_05 的物品 ID")]
    public string panel05ItemID = "ClockHourHand";

    private void Start()
    {
        RefreshInventory();
    }

    public void RefreshInventory()
    {
        if (InventoryManager.Instance == null ||
            itemList == null ||
            itemCardPrefab == null)
        {
            Debug.LogWarning("背包 UI 引用尚未设置完整！");
            return;
        }

        // 清理旧的背包格子
        foreach (Transform child in itemList)
        {
            Destroy(child.gameObject);
        }

        var items = InventoryManager.Instance.GetCollectedItems();

        foreach (CollectibleItem item in items)
        {
            if (item == null)
                continue;

            GameObject card = Instantiate(itemCardPrefab, itemList);

            // 设置格子尺寸
            RectTransform cardRect =
                card.GetComponent<RectTransform>();

            if (cardRect != null)
            {
                cardRect.sizeDelta =
                    new Vector2(cardWidth, cardHeight);
            }

            InventoryItemDrag drag =
                card.GetComponent<InventoryItemDrag>();

            if (drag != null)
            {
                drag.itemID = item.itemID;

                // 保留原有吹风机功能
                if (item.itemID == "HairDryer")
                {
                    drag.hairDryerController =
                        hairDryerController;
                }

                // 保留 Panel_05 投放功能
                if (item.itemID == panel05ItemID)
                {
                    drag.canDropOnPanel05 = true;
                    drag.panel05ItemController =
                        panel05ItemController;
                }
            }

            // 设置背包图标图片，不修改图标尺寸
            Transform iconTransform =
                card.transform.Find("ItemIcon");

            Image icon = iconTransform != null
                ? iconTransform.GetComponent<Image>()
                : null;

            if (icon != null)
            {
                icon.sprite = item.itemIcon;
                icon.enabled = item.itemIcon != null;
                icon.preserveAspect = true;
            }
        }
    }
}
