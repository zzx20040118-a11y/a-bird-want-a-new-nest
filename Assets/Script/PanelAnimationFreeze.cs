using UnityEngine;

public class PanelAnimationFreeze : MonoBehaviour
{
    private Animator animator;
    private bool isFrozen = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    // 点击成功后调用这个方法
    public void FreezeAnimation()
    {
        if (isFrozen) return;

        if (animator != null)
        {
            animator.speed = 0f;
            isFrozen = true;
        }
    }
}