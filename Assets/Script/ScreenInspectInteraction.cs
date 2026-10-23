using UnityEngine;

public class ScreenInspectInteraction : MonoBehaviour
{
    [Header("检查界面")]
    public GameObject inspectCanvas;

    [Header("图片缩放控制器")]
    public StoryCanvasZoomPan inspectImageZoomPan;

    [Header("第一人称控制器")]
    public FirstPersonController firstPersonController;

    [Header("可选：交互提示")]
    public GameObject interactionPrompt;

    private bool playerNearby;
    private bool isInspecting;

    private void Start()
    {
        if (inspectCanvas != null)
            inspectCanvas.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void Update()
    {
        if (isInspecting)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
                CloseInspect();

            return;
        }

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
            OpenInspect();
    }

    private void OpenInspect()
    {
        if (inspectCanvas == null)
        {
            Debug.LogError("请绑定 Inspect Canvas！");
            return;
        }

        // 每次打开都恢复图片最初的大小和位置
        if (inspectImageZoomPan != null)
            inspectImageZoomPan.ResetView();

        inspectCanvas.SetActive(true);
        isInspecting = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (firstPersonController != null)
            firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    private void CloseInspect()
    {
        if (inspectCanvas != null)
            inspectCanvas.SetActive(false);

        isInspecting = false;

        if (firstPersonController != null)
            firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        if (!isInspecting && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        if (!isInspecting && interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }
}
