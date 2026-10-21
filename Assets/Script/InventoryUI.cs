using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemList;
    public GameObject itemCardPrefab;

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

        // 清理旧的背包图标
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

                // 设置 Panel_05 投放功能
                if (item.itemID == panel05ItemID)
                {
                    drag.canDropOnPanel05 = true;
                    drag.panel05ItemController =
                        panel05ItemController;
                }
            }

            // 设置背包图标图片
            Transform iconTransform =
                card.transform.Find("ItemIcon");

            Image icon = iconTransform != null
                ? iconTransform.GetComponent<Image>()
                : null;

            if (icon != null)
            {
                icon.sprite = item.itemIcon;
                icon.enabled = item.itemIcon != null;
            }
        }
    }
}
