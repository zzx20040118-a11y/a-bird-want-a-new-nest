
using UnityEngine;

public class PlayerInteraction : MonoBehaviour
{
    [Header("交互设置")]
    public Transform playerCamera;
    public float interactDistance = 3f;

    private InteractableObject currentTarget;
    private OutlineEffect currentOutline;

    void Update()
    {
        UpdateTarget();

        if (Input.GetKeyDown(KeyCode.E) && currentTarget != null)
        {
            currentTarget.Interact();
        }
    }

    void UpdateTarget()
    {
        if (playerCamera == null)
        {
            Debug.LogError("PlayerInteraction：Player Camera 未指定！");
            return;
        }

        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;
        InteractableObject newTarget = null;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            Debug.Log("射线命中对象：" + hit.collider.name);

            newTarget = hit.collider.GetComponentInParent<InteractableObject>();

            if (newTarget != null)
            {
                Debug.Log("找到可交互物体：" + newTarget.name);
                Debug.Log("InteractableObject 所在对象：" + newTarget.gameObject.name);

                OutlineEffect sameObject = newTarget.GetComponent<OutlineEffect>();
                Debug.Log("同一对象上的 OutlineEffect：" + (sameObject != null ? "有" : "没有"));

                OutlineEffect childObject = newTarget.GetComponentInChildren<OutlineEffect>(true);
                Debug.Log("子物体上的 OutlineEffect：" + (childObject != null ? childObject.name : "没有"));
            }
        }

        if (newTarget == currentTarget)
            return;

        if (currentOutline != null)
        {
            currentOutline.SetOutline(false);
            Debug.Log("已关闭旧轮廓");
        }

        currentTarget = newTarget;
        currentOutline = null;

        if (currentTarget != null)
        {
            currentOutline = currentTarget.GetComponent<OutlineEffect>();

            if (currentOutline == null)
                currentOutline = currentTarget.GetComponentInParent<OutlineEffect>();

            if (currentOutline == null)
                currentOutline = currentTarget.GetComponentInChildren<OutlineEffect>(true);

            if (currentOutline != null)
            {
                Debug.Log("准备开启轮廓，组件对象：" + currentOutline.gameObject.name);
                currentOutline.SetOutline(true);
                Debug.Log("已调用 SetOutline(true)");
            }
            else
            {
                Debug.LogWarning("当前可交互物体及其父子层级均未找到 OutlineEffect！");
            }
        }
    }

    void OnDisable()
    {
        if (currentOutline != null)
        {
            currentOutline.SetOutline(false);
        }
    }
}
