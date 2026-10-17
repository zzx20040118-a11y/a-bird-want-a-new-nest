using UnityEngine;

public class MonitorInteraction : MonoBehaviour
{
    private bool playerNearby = false;
    private MonitorZoom monitorZoom;

    public GameObject interactionPrompt;

    void Start()
    {
        monitorZoom = GetComponentInParent<MonitorZoom>();

        if (monitorZoom == null)
        {
            monitorZoom = FindFirstObjectByType<MonitorZoom>();
        }

        if (interactionPrompt != null)
        {
            interactionPrompt.SetActive(false);
        }
    }

    void Update()
    {
        if (playerNearby && Input.GetKeyDown(KeyCode.E))
        {
            if (monitorZoom != null)
            {
                if (interactionPrompt != null)
                {
                    interactionPrompt.SetActive(false);
                }

                monitorZoom.StartZoom();
            }
            else
            {
                Debug.LogError("没有找到 MonitorZoom 组件！");
            }
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = true;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(true);
            }

            Debug.Log("按 E 与主屏幕交互");
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerNearby = false;

            if (interactionPrompt != null)
            {
                interactionPrompt.SetActive(false);
            }

            Debug.Log("离开主屏幕");
        }
    }
}
