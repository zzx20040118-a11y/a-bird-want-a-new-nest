using System.Collections;
using UnityEngine;

public class MonitorZoom : MonoBehaviour
{
    [Header("镜头推进设置")]
    public Transform playerCamera;
    public Transform monitor;

    public float zoomDuration = 1.2f;
    public float stopDistance = 0.8f;

    [Header("故事界面")]
    public StoryUIController storyUIController;
    public FirstPersonController firstPersonController;

    private bool isZooming = false;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool hasOriginalCameraTransform = false;

    public void StartZoom()
    {
        if (isZooming)
            return;

        if (playerCamera == null || monitor == null)
        {
            Debug.LogError(
                "请在 MonitorZoom 中指定 Player Camera 和 Monitor！"
            );
            return;
        }

        originalCameraPosition = playerCamera.position;
        originalCameraRotation = playerCamera.rotation;
        hasOriginalCameraTransform = true;

        if (firstPersonController != null)
        {
            firstPersonController.enabled = false;
        }

        StartCoroutine(ZoomRoutine());
    }

    private IEnumerator ZoomRoutine()
    {
        isZooming = true;

        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        // 使用监控屏幕反方向的水平轴向
        Vector3 forward = -monitor.forward;
        forward.y = 0f;

        if (forward.sqrMagnitude < 0.001f)
        {
            forward = -transform.forward;
            forward.y = 0f;
        }

        if (forward.sqrMagnitude < 0.001f)
        {
            Debug.LogError("无法确定水平推进方向！");
            isZooming = false;
            yield break;
        }

        forward.Normalize();

        // 沿反转后的水平轴向推进
        Vector3 targetPosition =
            monitor.position + forward * stopDistance;

        // 最终镜头朝向屏幕
        Vector3 lookDirection = -forward;

        Quaternion targetRotation = Quaternion.LookRotation(
            lookDirection,
            Vector3.up
        );

        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / zoomDuration);
            t = t * t * (3f - 2f * t);

            playerCamera.position = Vector3.Lerp(
                startPosition,
                targetPosition,
                t
            );

            playerCamera.rotation = Quaternion.Slerp(
                startRotation,
                targetRotation,
                t
            );

            yield return null;
        }

        playerCamera.position = targetPosition;
        playerCamera.rotation = targetRotation;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        if (storyUIController != null)
        {
            storyUIController.ShowStory();
        }
        else
        {
            Debug.LogError(
                "请在 MonitorZoom 中指定 Story UI Controller！"
            );
        }
    }

    public void ReturnToRoom()
    {
        if (storyUIController != null &&
            storyUIController.storyUI != null)
        {
            storyUIController.storyUI.SetActive(false);
        }

        if (hasOriginalCameraTransform && playerCamera != null)
        {
            playerCamera.position = originalCameraPosition;
            playerCamera.rotation = originalCameraRotation;
        }

        if (firstPersonController != null)
        {
            firstPersonController.enabled = true;
        }

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        isZooming = false;

        Debug.Log("已返回监控室");
    }
}
