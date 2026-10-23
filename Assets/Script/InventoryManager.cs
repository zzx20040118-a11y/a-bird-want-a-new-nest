using System.Collections.Generic;
using UnityEngine;

public class InventoryManager : MonoBehaviour
{
    public static InventoryManager Instance { get; private set; }

    private HashSet<string> collectedItems = new HashSet<string>();

    private Dictionary<string, CollectibleItem> itemDatabase =
        new Dictionary<string, CollectibleItem>();

    public bool HasItem(string itemID)
    {
        return collectedItems.Contains(itemID);
    }

    public void RegisterItem(CollectibleItem item)
    {
        if (item == null || string.IsNullOrEmpty(item.itemID))
            return;

        itemDatabase[item.itemID] = item;
    }

  public bool AddItem(string itemID)
{
    if (string.IsNullOrEmpty(itemID))
    {
        Debug.LogWarning("物品 ID 不能为空！");
        return false;
    }

    bool added = collectedItems.Add(itemID);

    Debug.Log(
        "[背包诊断] AddItem 被调用：" + itemID +
        "，是否新增：" + added +
        "，当前物品：" + string.Join(", ", collectedItems)
    );

    if (added)
    {
        Debug.Log("已收集物品：" + itemID);
        RefreshInventoryUI();
    }

    return added;
}

    public bool RemoveItem(string itemID)
    {
        if (string.IsNullOrEmpty(itemID))
            return false;

        bool removed = collectedItems.Remove(itemID);

        if (removed)
        {
            Debug.Log("已移除物品：" + itemID);
            RefreshInventoryUI();
        }

        return removed;
    }

    public List<CollectibleItem> GetCollectedItems()
    {
        List<CollectibleItem> result =
            new List<CollectibleItem>();

        foreach (string itemID in collectedItems)
        {
            if (itemDatabase.TryGetValue(
                itemID, out CollectibleItem item)
                && item != null)
            {
                result.Add(item);
            }
        }

        return result;
    }

    private void RefreshInventoryUI()
    {
        InventoryUI[] inventoryUIs =
            FindObjectsOfType<InventoryUI>(true);

        foreach (InventoryUI ui in inventoryUIs)
        {
            if (ui != null)
                ui.RefreshInventory();
        }
    }

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.I))
        {
            Debug.Log(
                "背包是否有时针：" +
                HasItem("ClockHourHand"));

            Debug.Log(
                "背包是否有分针：" +
                HasItem("ClockMinuteHand"));
        }
    }
}
