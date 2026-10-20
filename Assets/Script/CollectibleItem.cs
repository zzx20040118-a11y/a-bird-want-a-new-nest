using UnityEngine;

public class CollectibleItem : MonoBehaviour
{
    public string itemID = "ClockHourHand";
    public string itemName = "时针";
    public Sprite itemIcon;

    private bool playerInRange = false;

    private void Start()
    {
        if (InventoryManager.Instance != null)
        {
            InventoryManager.Instance.RegisterItem(this);
        }
    }

    private void Update()
    {
            if (Input.GetKeyDown(KeyCode.I))
    {
        if (InventoryManager.Instance != null)
        {
            Debug.Log(
                "背包是否有时针：" +
                InventoryManager.Instance.HasItem("ClockHourHand")
            );
        }
    }
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            if (InventoryManager.Instance == null)
            {
                Debug.LogWarning("场景中没有启用 InventoryManager！");
                return;
            }

            bool added = InventoryManager.Instance.AddItem(itemID);

            if (added)
            {
                Debug.Log("拾取成功：" + itemName);
                gameObject.SetActive(false);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            Debug.Log("靠近物品：" + itemName + "，按 E 拾取");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }
}
