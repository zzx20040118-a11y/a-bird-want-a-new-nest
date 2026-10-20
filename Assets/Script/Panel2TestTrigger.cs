using UnityEngine;

public class Panel2TestTrigger : MonoBehaviour
{
    public Animator panel2Animator;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.T))
        {
            Debug.Log("按下了 T");
            panel2Animator.SetTrigger("PopIn2");
        }
    }
}
