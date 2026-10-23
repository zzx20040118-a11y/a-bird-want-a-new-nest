using UnityEngine;

public class InteractableObject : MonoBehaviour
{
    [Header("交互设置")]
    public string interactionName = "调查物品";

    public virtual void Interact()
    {
        Debug.Log("正在交互：" + interactionName);
    }
}
