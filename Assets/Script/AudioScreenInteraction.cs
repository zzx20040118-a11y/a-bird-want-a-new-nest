
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class AudioScreenInteraction : MonoBehaviour
{
    [Header("音频界面")]
    public GameObject audioCanvas;

    [Header("背包界面")]
    public GameObject inventoryCanvas;

    [Header("音频播放器")]
    public AudioSource audioSource;

    [Header("播放/暂停按钮")]
    public Button playPauseButton;
    public Text playPauseButtonText;
    public TMP_Text playPauseTMPText;

    [Header("音频进度条")]
    public Slider audioProgressSlider;

    [Header("扫描线")]
    public RectTransform scanLine;
    public RectTransform audioImage;

    [Header("扫描线左右边距")]
    public float scanLeftMargin = 15f;
    public float scanRightMargin = 15f;

    [Header("第一人称控制器")]
    public FirstPersonController firstPersonController;

    [Header("可选：交互提示")]
    public GameObject interactionPrompt;

    private bool playerNearby;
    private bool isOpen;
    private bool isUpdatingSlider;

    // 记录打开音频界面前背包是否显示
    private bool inventoryWasVisible;

    private void Start()
    {
        if (audioCanvas != null)
            audioCanvas.SetActive(false);

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (playPauseButton != null)
            playPauseButton.onClick.AddListener(TogglePlayback);

        if (audioProgressSlider != null)
        {
            audioProgressSlider.minValue = 0f;
            audioProgressSlider.maxValue = 1f;
            audioProgressSlider.SetValueWithoutNotify(0f);

            audioProgressSlider.onValueChanged.AddListener(
                OnProgressChanged
            );
        }

        UpdateButtonText();
        UpdateScanLine(0f);
    }

    private void Update()
    {
        if (isOpen)
        {
            UpdateAudioProgress();

            if (Input.GetKeyDown(KeyCode.Escape))
                CloseAudio();

            return;
        }

        if (playerNearby && Input.GetKeyDown(KeyCode.E))
            OpenAudio();
    }

    private void OpenAudio()
    {
        if (audioCanvas == null)
        {
            Debug.LogError("请绑定 AudioCanvas！");
            return;
        }

        // 记录背包原本的显示状态
        inventoryWasVisible =
            inventoryCanvas != null && inventoryCanvas.activeSelf;

        // 打开音频界面时隐藏背包
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(false);

        audioCanvas.SetActive(true);
        isOpen = true;

        if (interactionPrompt != null)
            interactionPrompt.SetActive(false);

        if (firstPersonController != null)
            firstPersonController.enabled = false;

        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;

        UpdateAudioProgress();
        UpdateButtonText();
    }

    private void CloseAudio()
    {
        if (audioSource != null)
            audioSource.Stop();

        if (audioProgressSlider != null)
            audioProgressSlider.SetValueWithoutNotify(0f);

        UpdateScanLine(0f);

        if (audioCanvas != null)
            audioCanvas.SetActive(false);

        // 恢复背包之前的显示状态
        if (inventoryCanvas != null)
            inventoryCanvas.SetActive(inventoryWasVisible);

        isOpen = false;

        if (firstPersonController != null)
            firstPersonController.enabled = true;

        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;

        if (interactionPrompt != null && playerNearby)
            interactionPrompt.SetActive(true);

        UpdateButtonText();
    }

    private void TogglePlayback()
    {
        if (!isOpen ||
            audioSource == null ||
            audioSource.clip == null)
        {
            Debug.LogWarning("请检查 AudioSource 和 AudioClip！");
            return;
        }

        if (audioSource.isPlaying)
        {
            audioSource.Pause();
        }
        else
        {
            if (audioSource.time >= audioSource.clip.length)
                audioSource.time = 0f;

            audioSource.UnPause();

            if (!audioSource.isPlaying)
                audioSource.Play();
        }

        UpdateButtonText();
    }

    private void UpdateAudioProgress()
    {
        if (audioSource == null ||
            audioSource.clip == null ||
            audioProgressSlider == null)
            return;

        float duration = audioSource.clip.length;

        if (duration <= 0f)
            return;

        float progress = Mathf.Clamp01(
            audioSource.time / duration
        );

        isUpdatingSlider = true;
        audioProgressSlider.SetValueWithoutNotify(progress);
        isUpdatingSlider = false;

        UpdateScanLine(progress);
        UpdateButtonText();
    }

    private void OnProgressChanged(float value)
    {
        if (isUpdatingSlider ||
            !isOpen ||
            audioSource == null ||
            audioSource.clip == null)
            return;

        float duration = audioSource.clip.length;

        if (duration <= 0f)
            return;

        audioSource.time = Mathf.Clamp(
            value * duration,
            0f,
            duration
        );

        UpdateScanLine(value);
    }

    private void UpdateScanLine(float progress)
    {
        if (scanLine == null || audioImage == null)
            return;

        float halfWidth = audioImage.rect.width * 0.5f;

        float leftLimit = -halfWidth + scanLeftMargin;
        float rightLimit = halfWidth - scanRightMargin;

        // 防止左右边距过大导致移动范围反转
        if (leftLimit > rightLimit)
        {
            float middle = (leftLimit + rightLimit) * 0.5f;
            leftLimit = middle;
            rightLimit = middle;
        }

        Vector2 position = scanLine.anchoredPosition;

        position.x = Mathf.Lerp(
            leftLimit,
            rightLimit,
            Mathf.Clamp01(progress)
        );

        scanLine.anchoredPosition = position;
    }

    private void UpdateButtonText()
    {
        string label =
            audioSource != null && audioSource.isPlaying
            ? "PAUSE"
            : "PLAY";

        if (playPauseButtonText != null)
            playPauseButtonText.text = label;

        if (playPauseTMPText != null)
            playPauseTMPText.text = label;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = true;

        if (!isOpen && interactionPrompt != null)
            interactionPrompt.SetActive(true);
    }

    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player"))
            return;

        playerNearby = false;

        if (!isOpen && interactionPrompt != null)
            interactionPrompt.SetActive(false);
    }

    private void OnDestroy()
    {
        if (playPauseButton != null)
            playPauseButton.onClick.RemoveListener(TogglePlayback);

        if (audioProgressSlider != null)
        {
            audioProgressSlider.onValueChanged.RemoveListener(
                OnProgressChanged
            );
        }
    }
}
