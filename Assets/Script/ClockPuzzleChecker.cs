using UnityEngine;

public class ClockPuzzleChecker : MonoBehaviour
{
    [Header("Clock Hands")]
    public RectTransform hourHand;
    public RectTransform minuteHand;

    [Header("Target Rotation Z")]
    public float targetHourRotationZ = 7.5f;
    public float targetMinuteRotationZ = 90f;
    public float angleTolerance = 5f;

    [Header("背包物品 ID")]
    public string hourHandItemID = "ClockHourHand";
    public string minuteHandItemID = "ClockMinuteHand";

    private bool puzzleSolved = false;

    private void Update()
    {
        if (puzzleSolved)
            return;

        if (hourHand == null || minuteHand == null)
            return;

        float hourZ = hourHand.localEulerAngles.z;
        float minuteZ = minuteHand.localEulerAngles.z;

        bool hourCorrect =
            Mathf.Abs(Mathf.DeltaAngle(
                hourZ, targetHourRotationZ)) <= angleTolerance;

        bool minuteCorrect =
            Mathf.Abs(Mathf.DeltaAngle(
                minuteZ, targetMinuteRotationZ)) <= angleTolerance;

        if (Input.GetKeyDown(KeyCode.P))
        {
            Debug.Log(
                $"时针 Z={hourZ:F1}，目标={targetHourRotationZ:F1}，正确={hourCorrect}"
            );

            Debug.Log(
                $"分针 Z={minuteZ:F1}，目标={targetMinuteRotationZ:F1}，正确={minuteCorrect}"
            );
        }

        if (hourCorrect && minuteCorrect)
        {
            puzzleSolved = true;

            Debug.Log("钟表谜题解开！");

            // 谜题成功后，从背包数据中移除时针和分针
            if (InventoryManager.Instance != null)
            {
                InventoryManager.Instance.RemoveItem(hourHandItemID);
                InventoryManager.Instance.RemoveItem(minuteHandItemID);

                Debug.Log("已移除背包中的时针和分针。");
            }
            else
            {
                Debug.LogWarning(
                    "没有找到 InventoryManager，无法移除时针和分针。"
                );
            }

            // 保留原有动画逻辑
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
                    "没有找到 ClockAnimationRoot，请检查层级！"
                );
                return;
            }

            animationRoot.gameObject.SetActive(true);

            Animator animator =
                animationRoot.GetComponent<Animator>();

            if (animator != null)
            {
                animator.SetTrigger("ClockSolved");
                Debug.Log("已触发 ClockSolved！");
            }
            else
            {
                Debug.LogWarning(
                    "ClockAnimationRoot 上没有 Animator！"
                );
            }
        }
    }
}
