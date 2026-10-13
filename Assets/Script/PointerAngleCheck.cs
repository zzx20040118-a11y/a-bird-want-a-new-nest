using UnityEngine;

public class PointerAngleCheck : MonoBehaviour
{
    public RectTransform pointer;
    public RectTransform targetPoint;

    [Range(0f, 45f)]
    public float tolerance = 5f;

    public bool IsCorrect()
    {
        if (pointer == null || targetPoint == null)
            return false;

        Vector3 pivot = pointer.TransformPoint(
            new Vector3(0f, 0f, 0f));

        Vector3 tip = pointer.TransformPoint(
            new Vector3(0f, pointer.rect.height, 0f));

        Vector3 target = targetPoint.position;

        Vector2 pointerDirection = tip - pivot;
        Vector2 targetDirection = target - pivot;

        float angleDifference = Vector2.Angle(
            pointerDirection, targetDirection);

        return angleDifference <= tolerance;
    }

    private void Update()
    {
        if (IsCorrect())
        {
            Debug.Log("Pointer is aligned with target!");
        }
    }
}
