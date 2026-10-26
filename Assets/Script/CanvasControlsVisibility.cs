
using UnityEngine;

public class CanvasControlsVisibility : MonoBehaviour
{
    [Header("画布 A：显示两个控件")]
    public GameObject canvasA;

    [Header("画布 B：只显示控件 1")]
    public GameObject canvasB;

    [Header("独立的 UI 控件")]
    public GameObject control1;
    public GameObject control2;

    private void Update()
    {
        bool showBoth = canvasA != null &&
                        canvasA.activeInHierarchy;

        bool showOnlyOne = canvasB != null &&
                           canvasB.activeInHierarchy;

        if (control1 != null)
        {
            control1.SetActive(showBoth || showOnlyOne);
        }

        if (control2 != null)
        {
            control2.SetActive(showBoth);
        }
    }
}
