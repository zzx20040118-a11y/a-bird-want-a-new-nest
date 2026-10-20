using System.Collections;
using UnityEngine;

public class MonitorZoom : MonoBehaviour
{
    public Transform playerCamera;
    public Transform monitor;

    public float zoomDuration = 1.2f;
    public float stopDistance = 0.8f;

    public StoryUIController storyUIController;
    public FirstPersonController firstPersonController;

    private bool isZooming = false;

    private Vector3 originalCameraPosition;
    private Quaternion originalCameraRotation;
    private bool hasOriginalCameraTransform = false;

    
    public void StartZoom()
    {
        if (!isZooming)
        {
            originalCameraPosition = playerCamera.position;
            originalCameraRotation = playerCamera.rotation;
            hasOriginalCameraTransform = true;

            if (firstPersonController != null)
            {
                firstPersonController.enabled = false;
            }

            StartCoroutine(ZoomRoutine());
        }
    }


    private IEnumerator ZoomRoutine()
    {
        isZooming = true;

        Vector3 startPosition = playerCamera.position;
        Quaternion startRotation = playerCamera.rotation;

        Vector3 direction = monitor.position - playerCamera.position;
        direction.Normalize();

        Vector3 targetPosition =
            monitor.position - direction * stopDistance;

        Quaternion targetRotation = Quaternion.LookRotation(direction);

        float elapsed = 0f;

        while (elapsed < zoomDuration)
        {
            elapsed += Time.deltaTime;

            float t = Mathf.Clamp01(elapsed / zoomDuration);
            t = t * t * (3f - 2f * t);

            playerCamera.position =
                Vector3.Lerp(startPosition, targetPosition, t);

            playerCamera.rotation =
                Quaternion.Slerp(startRotation, targetRotation, t);

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
            Debug.LogError("请在 MonitorZoom 中指定 Story UI Controller！");
        }
    }

    

    
    
    public void ReturnToRoom()
    {
        if (storyUIController != null && storyUIController.storyUI != null)
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