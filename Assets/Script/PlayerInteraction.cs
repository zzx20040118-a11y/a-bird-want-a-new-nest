
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
        Ray ray = new Ray(playerCamera.position, playerCamera.forward);
        RaycastHit hit;

        InteractableObject newTarget = null;

        if (Physics.Raycast(ray, out hit, interactDistance))
        {
            newTarget = hit.collider
                .GetComponentInParent<InteractableObject>();
        }

        if (newTarget == currentTarget)
            return;

        if (currentOutline != null)
        {
            currentOutline.SetOutline(false);
        }

        currentTarget = newTarget;
        currentOutline = null;

        if (currentTarget != null)
        {
            currentOutline = currentTarget
                .GetComponent<OutlineEffect>();

            if (currentOutline != null)
            {
                currentOutline.SetOutline(true);
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

