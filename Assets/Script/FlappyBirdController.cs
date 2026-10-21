using UnityEngine;

public class FlappyBirdController : MonoBehaviour
{
    [Header("跳跃力度")]
    public float jumpForce = 350f;

    [Header("下落速度")]
    public float gravity = 900f;

    private RectTransform rectTransform;
    private float verticalSpeed = 0f;

    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    private void Update()
    {
        // 鼠标左键点击，小鸟向上跳
        if (Input.GetMouseButtonDown(0))
        {
            verticalSpeed = jumpForce;
        }

        // 重力让小鸟逐渐下落
        verticalSpeed -= gravity * Time.deltaTime;

        // 更新小鸟的垂直位置
        rectTransform.anchoredPosition +=
            Vector2.up * verticalSpeed * Time.deltaTime;
    }
}
