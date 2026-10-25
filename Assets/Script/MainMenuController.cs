
using System.Collections;
using UnityEngine;

public class MainMenuController : MonoBehaviour
{
    [Header("主菜单摄像机")]
    public Camera mainMenuCamera;

    [Header("玩家摄像机")]
    public Camera playerCamera;

    [Header("主菜单界面")]
    public GameObject mainMenuCanvas;

    [Header("背包界面")]
    public GameObject inventoryCanvas;

    [Header("玩家控制器")]
    public FirstPersonController firstPersonController;

    [Header("开局固定视角时间")]
    public float introDuration = 2f;

    private bool gameStarted;

    private void Start()
    {
        // 标题页面使用主菜单摄像机
        if (mainMenuCamera != null)
            mainMenuCamera.enabled = true;

        if (playerCamera != null)
            playerCamera.enabled = false;

        // 标题页面不允许控制角色
        if (firstPersonController != null)
            firstPersonController.enabled = false;

        // 显示主菜单
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(true);

        // 标题页面隐藏背包
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false);

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    // 绑定到 START 按钮
    public void StartGame()
    {
        if (gameStarted)
            return;

        gameStarted = true;
        StartCoroutine(SwitchToPlayerView());
    }

    private IEnumerator SwitchToPlayerView()
    {
        // 隐藏主菜单
        if (mainMenuCanvas != null)
            mainMenuCanvas.SetActive(false);

        // 切换到玩家摄像机
        if (mainMenuCamera != null)
            mainMenuCamera.enabled = false;

        if (playerCamera != null)
            playerCamera.enabled = true;

        // 暂时禁止玩家移动和视角控制
        if (firstPersonController != null)
            firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        // 固定视角停顿
        yield return new WaitForSeconds(introDuration);

        // 停顿结束，显示背包
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(true);

        // 开始第一人称控制
        if (firstPersonController != null)
            firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
