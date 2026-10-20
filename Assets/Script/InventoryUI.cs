
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    public GameObject inventoryPanel;
    public Transform itemList;
    public GameObject itemCardPrefab;

    [Header("吹风机动画控制器")]
    public HairDryerAnimationController hairDryerController;

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

        foreach (Transform child in itemList)
        {
            Destroy(child.gameObject);
        }

        var items = InventoryManager.Instance.GetCollectedItems();

        foreach (CollectibleItem item in items)
        {
            GameObject card = Instantiate(itemCardPrefab, itemList);

            InventoryItemDrag drag =
                card.GetComponent<InventoryItemDrag>();

            if (drag != null)
            {
                drag.itemID = item.itemID;

                // 自动设置运行时生成的 ItemCard 的动画控制器
                if (item.itemID == "HairDryer")
                {
                    drag.hairDryerController = hairDryerController;
                }
            }

            Transform iconTransform = card.transform.Find("ItemIcon");
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
