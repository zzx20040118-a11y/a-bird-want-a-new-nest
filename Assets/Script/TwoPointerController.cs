using UnityEngine;

public class TwoPointerController : MonoBehaviour
{
    public PointerAngleCheck pointer1Check;
    public PointerAngleCheck pointer2Check;

    public Animator animator;

    private bool hasTriggered = false;

    private void Update()
    {
        if (hasTriggered) return;

        if (pointer1Check != null &&
            pointer2Check != null &&
            pointer1Check.IsCorrect() &&
            pointer2Check.IsCorrect())
        {
            hasTriggered = true;
            animator.SetTrigger("Shack");
        }
    }
}
