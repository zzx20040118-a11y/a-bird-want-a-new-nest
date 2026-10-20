using UnityEngine;
using UnityEngine.EventSystems;

public class ClockDropZone : MonoBehaviour, IDropHandler
{
    public GameObject hourHandImage;
    public GameObject minuteHandImage;

    public void OnDrop(PointerEventData eventData)
    {
        if (eventData.pointerDrag == null)
            return;

        InventoryItemDrag draggedItem =
            eventData.pointerDrag.GetComponent<InventoryItemDrag>();

        if (draggedItem == null)
        {
            Debug.Log("放入的不是背包物品卡片。");
            return;
        }

        Debug.Log("拖入物品 ID：" + draggedItem.itemID);

        if (draggedItem.itemID == "ClockHourHand")
        {
           hourHandImage.SetActive(true);
           Destroy(draggedItem.gameObject);
           Debug.Log("时针已放置！");
        }
        else if (draggedItem.itemID == "ClockMinuteHand")
        {
            minuteHandImage.SetActive(true);
            Destroy(draggedItem.gameObject);
            Debug.Log("分针已放置！");
        }
    }
}