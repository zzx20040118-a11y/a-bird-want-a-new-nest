
using UnityEngine;

public class BirdMovement : MonoBehaviour
{
    public Camera gameCamera;
    public float moveSpeed = 3f;

    private Vector3 targetPosition;
    private bool isMoving;
    private float groundY;

    void Start()
    {
        groundY = transform.position.y;
        targetPosition = transform.position;

        if (gameCamera == null)
            gameCamera = Camera.main;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0) && gameCamera != null)
        {
            Vector3 mousePosition = Input.mousePosition;
            mousePosition.z = -gameCamera.transform.position.z;

            Vector3 worldPosition =
                gameCamera.ScreenToWorldPoint(mousePosition);

            targetPosition = new Vector3(
                worldPosition.x,
                groundY,
                transform.position.z
            );

            isMoving = true;

            Vector3 scale = transform.localScale;
            scale.x = Mathf.Abs(scale.x) *
                (targetPosition.x < transform.position.x ? -1 : 1);
            transform.localScale = scale;
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Vector3.Distance(transform.position, targetPosition) < 0.01f)
                isMoving = false;
        }
    }
}
