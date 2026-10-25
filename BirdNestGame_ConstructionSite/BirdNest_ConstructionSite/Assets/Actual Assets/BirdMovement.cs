
using UnityEngine;
using UnityEngine.SceneManagement;

public class BirdMovement : MonoBehaviour
{
    public Camera gameCamera;
    public float moveSpeed = 3f;

    [Header("Scene Transition")]
    public float exitX = 8f;
    public string nextSceneName = "ConstructionScene2";

    private Vector3 targetPosition;
    private bool isMoving = false;
    private float groundY;

    void Start()
    {
        groundY = transform.position.y;
        targetPosition = transform.position;
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
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

            if (targetPosition.x != transform.position.x)
            {
                Vector3 scale = transform.localScale;
                scale.x = Mathf.Abs(scale.x) *
                    (targetPosition.x < transform.position.x ? -1 : 1);
                transform.localScale = scale;
            }
        }

        if (isMoving)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                targetPosition,
                moveSpeed * Time.deltaTime
            );

            if (Mathf.Abs(transform.position.x - targetPosition.x)
                < 0.01f)
            {
                isMoving = false;

                if (transform.position.x >= exitX)
                {
                    SceneManager.LoadScene(
                        nextSceneName,
                        LoadSceneMode.Single
                    );
                }
            }
        }
    }
}
