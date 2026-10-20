
using UnityEngine;

public class ClockPuzzleChecker : MonoBehaviour
{
    [Header("Clock Hands")]
    public RectTransform hourHand;
    public RectTransform minuteHand;

    [Header("Target Time")]
    public float targetHourAngle = 82.5f;
    public float targetMinuteAngle = 0f;
    public float angleTolerance = 5f;

    private bool puzzleSolved = false;

    private void Update()
    {
        if (puzzleSolved)
            return;

        if (hourHand == null || minuteHand == null)
            return;

        float hourAngle = GetClockAngle(hourHand);
        float minuteAngle = GetClockAngle(minuteHand);

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log("时针实际角度：" + hourAngle);
            Debug.Log("分针实际角度：" + minuteAngle);
        }

        bool hourCorrect =
            Mathf.Abs(Mathf.DeltaAngle(hourAngle, targetHourAngle))
            <= angleTolerance;

        bool minuteCorrect =
            Mathf.Abs(Mathf.DeltaAngle(minuteAngle, targetMinuteAngle))
            <= angleTolerance;

        if (hourCorrect && minuteCorrect)
        {
            puzzleSolved = true;
            Debug.Log("钟表谜题解开！目标时间：11:45");

            Transform panel = transform.parent;

            if (panel == null)
            {
                Debug.LogWarning("ClockPuzzleChecker 没有父物体！");
                return;
            }

            Transform animationRoot =
                panel.Find("ClockAnimationRoot");

            if (animationRoot == null)
            {
                Debug.LogWarning(
                    "没有找到 ClockAnimationRoot，请检查物体层级！"
                );
                return;
            }

            // 先激活动画物体
            animationRoot.gameObject.SetActive(true);

            Animator animator =
                animationRoot.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("ClockSolved");
                Debug.Log("已激活并触发 ClockAnimationRoot 动画！");
            }
            else
            {
                Debug.LogWarning(
                    "ClockAnimationRoot 上没有 Animator！"
                );
            }
        }
        else if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(
                "时针是否正确：" + hourCorrect +
                "，分针是否正确：" + minuteCorrect
            );
        }
    }

    private float GetClockAngle(RectTransform hand)
    {
        return Mathf.Repeat(
            90f - hand.localEulerAngles.z,
            360f
        );
    }
}
