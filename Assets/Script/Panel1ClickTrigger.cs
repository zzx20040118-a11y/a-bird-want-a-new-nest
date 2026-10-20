using UnityEngine;
using UnityEngine.UI;

public class Panel1ClickTrigger : MonoBehaviour
{
    public Animator panel2Animator;

    private Button button;

    void Awake()
    {
        button = GetComponent<Button>();
        button.onClick.AddListener(ShowPanel2);
    }

    void ShowPanel2()
    {
        if (panel2Animator != null)
        {
            panel2Animator.SetTrigger("PopIn2");
        }
        else
        {
            Debug.LogError("Panel2 Animator 尚未绑定！");
        }
    }
}
