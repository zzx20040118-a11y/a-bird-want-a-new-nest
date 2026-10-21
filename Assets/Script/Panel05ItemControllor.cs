using UnityEngine;

public class Panel05ItemController : MonoBehaviour
{
    [Header("投放成功后显示的场景物体")]
    public GameObject spawnedObject;

    [Header("必须投放的物品 ID")]
    public string requiredItemID;

    private bool hasBeenPlaced;

    public void PlaceItem(GameObject inventoryItem)
    {
        if (hasBeenPlaced)
            return;

        if (spawnedObject == null)
        {
            Debug.LogError(
                "Panel05ItemController：请指定 Spawned Object！"
            );
            return;
        }

        InventoryItemDrag drag =
            inventoryItem != null
                ? inventoryItem.GetComponent<InventoryItemDrag>()
                : null;

        if (drag == null)
        {
            Debug.LogError("背包图标上没有 InventoryItemDrag！");
            return;
        }

        // 如果设置了指定 ID，就检查投放的物品是否正确
        if (!string.IsNullOrEmpty(requiredItemID) &&
            drag.itemID != requiredItemID)
        {
            Debug.Log("这个物品不能投放到当前区域。");
            return;
        }

        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RemoveItem(drag.itemID);

            InventoryUI inventoryUI =
                FindObjectOfType<InventoryUI>();

            if (inventoryUI != null)
                inventoryUI.RefreshInventory();
        }

        hasBeenPlaced = true;
        spawnedObject.SetActive(true);

        Debug.Log("Panel_05：物品投放成功！");
    }
}
