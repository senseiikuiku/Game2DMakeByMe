using System.Globalization;
using TMPro;
using UnityEngine;
public class UIManager : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreKeyText; // hiển thị số lượng khóa đã thu thập
    [SerializeField] public TextMeshProUGUI liveText;

    [SerializeField] private GameObject gameOverUI;

    [SerializeField] private GameObject gameWinUI;

    [SerializeField] private GameObject cartUI; // Giao diện hiện thị giỏ hàng

    [SerializeField] public GameObject musicUI;
    [SerializeField] private GameObject musicBtn; // Nút để mở giao diện Music

    [SerializeField] private GameObject pauseUI; // Giao diện tạm dừng

    [SerializeField] private GameObject resumeBtn; // Nút tiếp tục trò chơi trong giao diện Pause

    [SerializeField] private GameObject menuBtn; // Nút trở về menu trong giao diện Pause

    [SerializeField] private GameObject restartGameBtn; // Nút chuyển đổi nhạc trong giao diện Pause

    [SerializeField] public GameObject runEffect; // Hiệu ứng chạy (nếu cần, có thể để trống nếu không sử dụng)

    public static UIManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        // Không cần DontDestroyOnLoad nếu UI mỗi scene khác nhau
    }


    void Start()
    {
        HideAllUI(); // Ẩn tất cả giao diện khi bắt đầu
        UpdateScore(GameManager.Instance.score);
        UpdateScoreKey(GameManager.Instance.scoreKey);
        UpdateLive(GameManager.Instance.countLive);
    }

    void Update()
    {

    }

    public void UpdateScore(int score)
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    public void UpdateScoreKey(int scoreKey)
    {
        if (scoreKeyText != null)
            scoreKeyText.text = scoreKey.ToString();
    }

    public void UpdateLive(int live)
    {
        if (liveText != null)
            liveText.text = live.ToString();
    }

    public void ShowGameOverUI()
    {
        if (gameOverUI != null)
            gameOverUI.SetActive(true);
    }

    public void ShowGameWinUI()
    {
        if (gameWinUI != null)
            gameWinUI.SetActive(true);
    }

    public void ToggleCartUI()
    {
        if (cartUI != null)
        {
            bool isActive = cartUI.activeSelf;
            cartUI.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;
        }
    }

    public void TogglePauseUI()
    {
        if (pauseUI != null)
        {
            bool isActive = pauseUI.activeSelf;
            pauseUI.SetActive(!isActive);
            Time.timeScale = isActive ? 1 : 0;
        }
    }

    public void ClosePauseUI()
    {
        if (pauseUI != null)
        {
            pauseUI.SetActive(false);
            Time.timeScale = 1;
        }
    }

    public void RestartGameBtn()
    {
        if (restartGameBtn != null)
        {
            // Thực hiện logic khởi động lại trò chơi
            GameManager.Instance?.RestartGame();
            ClosePauseUI();
        }
    }

    public void MenuBtn()
    {
        if (menuBtn != null)
        {
            // Thực hiện logic trở về menu
            GameManager.Instance?.GotoMenu();
            ClosePauseUI();
        }
    }

    public void ResumeBtn()
    {
        if (resumeBtn != null)
        {
            // Thực hiện logic tiếp tục trò chơi
            ClosePauseUI();
            Time.timeScale = 1; // Đặt lại thời gian trò chơi
        }
    }

    public void ToggleMusicUI()
    {
        if (musicUI != null)
        {
            bool isActive = musicUI.activeSelf;
            musicUI.SetActive(!isActive);
            ClosePauseUI();
            Time.timeScale = isActive ? 1 : 0;
        }
    }

    public void ToggleRunEffect(bool state)
    {
        if (runEffect != null)
            runEffect.SetActive(state);
    }

    private void HideAllUI()
    {
        if (gameOverUI != null) gameOverUI.SetActive(false);
        if (gameWinUI != null) gameWinUI.SetActive(false);
        if (cartUI != null) cartUI.SetActive(false);
        if (pauseUI != null) pauseUI.SetActive(false);
        if (musicUI != null) musicUI.SetActive(false);
        if (runEffect != null) runEffect.SetActive(false);
    }
}
