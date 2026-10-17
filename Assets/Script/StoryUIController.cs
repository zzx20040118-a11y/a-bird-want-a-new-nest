using System.Collections;
using UnityEngine;

public class StoryUIController : MonoBehaviour
{
    public GameObject storyUI;
    public Animator panelAnimator;
    public RectTransform panelRect;

    public void ShowStory()
{
    storyUI.SetActive(true);
    if (panelRect != null)
{
    panelRect.localScale = Vector3.zero;
}

    if (panelAnimator != null)
    {
        panelAnimator.ResetTrigger("PopIn");
        panelAnimator.Play("PanelIdle", 0, 0f);
    }

    StartCoroutine(PlayPanelAfterDelay());
}

    private IEnumerator PlayPanelAfterDelay()
    {
        // 进入画面后先停顿 1 秒
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
}