using UnityEngine;
using UnityEngine.EventSystems;

public class Panel05SpawnedItemClick : MonoBehaviour,
    IPointerClickHandler
{
    [Header("点击后要打开的窗口")]
    public GameObject nextPanel;

    public void OnPointerClick(PointerEventData eventData)
    {
        if (nextPanel == null)
        {
            Debug.LogError(
                "Panel05SpawnedItemClick：请指定 Next Panel！"
            );
            return;
        }

        nextPanel.SetActive(true);

        Debug.Log("Panel_05：已打开下一个故事窗口！");
    }
}
