using UnityEngine;
using System.Collections;

public class ImageSceneSwitcher : MonoBehaviour
{
    public GameObject scene1;
    public GameObject scene2;
    public Animator zoomAnimator;

    private bool isSwitching = false;

    public void SwitchImage()
    {
        if (isSwitching) return;

        isSwitching = true;
        zoomAnimator.SetTrigger("ZoomIn");

        StartCoroutine(WaitForZoom());
    }

    private IEnumerator WaitForZoom()
    {
        // Wait until the zoom animation starts
        yield return null;

        // Wait until the animator enters the ZoomIn state
        while (!zoomAnimator.GetCurrentAnimatorStateInfo(0)
                   .IsName("ZoomIn"))
        {
            yield return null;
        }

        // Wait until the animation finishes
        while (zoomAnimator.GetCurrentAnimatorStateInfo(0).normalizedTime < 1f)
        {
            yield return null;
        }

        scene1.SetActive(false);
        scene2.SetActive(true);
    }
}
