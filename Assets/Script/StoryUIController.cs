
using System.Collections;
using UnityEngine;

public class StoryUIController : MonoBehaviour
{
    public GameObject storyUI;
    public Animator panelAnimator;
    public RectTransform panelRect;
    public MonitorZoom monitorZoom;

    private bool hasShownFirstPanel = false;

    public void ShowStory()
    {
        Debug.Log("ShowStory 被调用了：" + Time.time);

        storyUI.SetActive(true);

        if (!hasShownFirstPanel)
        {
            // 第一次进入：播放第一张画面的弹出动画
            if (panelRect != null)
            {
                panelRect.localScale = Vector3.zero;
            }

            if (panelAnimator != null)
            {
                panelAnimator.enabled = true;
                panelAnimator.ResetTrigger("PopIn");
                panelAnimator.Play("PanelHidden", 0, 0f);
            }

            StartCoroutine(PlayPanelAfterDelay());
            hasShownFirstPanel = true;
        }
        else
        {
            // 再次进入：直接显示第一张画面，不播放动画
            if (panelRect != null)
            {
                panelRect.localScale = Vector3.one * 5f;
            }

            if (panelAnimator != null)
            {
                panelAnimator.enabled = false;
            }
        }
    }

    private IEnumerator PlayPanelAfterDelay()
    {
        yield return new WaitForSeconds(1f);

        if (panelAnimator != null)
        {
            panelAnimator.SetTrigger("PopIn");
        }
        else
        {
            Debug.LogError("请指定 Panel Animator！");
        }
    }

    private void Update()
    {
        if (storyUI != null &&
            storyUI.activeSelf &&
            Input.GetKeyDown(KeyCode.Escape))
        {
            if (monitorZoom != null)
            {
                monitorZoom.ReturnToRoom();
            }
            else
            {
                storyUI.SetActive(false);
                Debug.LogWarning("尚未指定 Monitor Zoom！");
            }
        }
    }
}
