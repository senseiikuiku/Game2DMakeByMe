using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public int score = 0;
    private int scoreKey = 0; //Theo dõi số lượng chìa khóa đã thu thập
    [SerializeField] private TextMeshProUGUI scoreText;
    [SerializeField] private TextMeshProUGUI scoreKeyText; // hiển thị số lượng khóa đã thu thập

    [SerializeField] public TextMeshProUGUI live;
    public int countLive = 3; // Số mạng mà người chơi có

    [SerializeField] private GameObject gameOverUI;
    private bool isGameOver = false;

    [SerializeField] private GameObject gameWinUI;
    private bool isGameWin = false;

    [SerializeField] private GameObject cartUI; // Giao diện hiện thị giỏ hàng

    [SerializeField] private GameObject musicUI;
    [SerializeField] private GameObject musicBtn; // Nút để mở giao diện Music

    [SerializeField] private GameObject pauseUI; // Giao diện tạm dừng

    [SerializeField] private GameObject resumeBtn; // Nút tiếp tục trò chơi trong giao diện Pause

    [SerializeField] private GameObject menuBtn; // Nút trở về menu trong giao diện Pause

    [SerializeField] private GameObject restartGameBtn; // Nút chuyển đổi nhạc trong giao diện Pause

    private PlayerController playerController; // Tham chiếu đến PlayerController để xử lý các hành động của người chơi

    [SerializeField] public GameObject runEffect; // Hiệu ứng chạy (nếu cần, có thể để trống nếu không sử dụng)


    public static GameManager Instance { get; private set; }
    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;
        DontDestroyOnLoad(gameObject); // Giữ GameManager tồn tại khi chuyển cảnh
        playerController = FindAnyObjectByType<PlayerController>();
    }



    void Start()
    {
        UpdateLive();
        UpdateScore();
        UpdateScoreKey();
        if (gameOverUI != null)
            gameOverUI.SetActive(false); // Ẩn giao diện Game Over khi bắt đầu
        if (gameWinUI != null)
            gameWinUI.SetActive(false); // Ân giao diện Game Win khi bắt đầu
        if (cartUI != null)
            cartUI.SetActive(false); // Ẩn giao diện giỏ hàng khi bắt đầu
        if (pauseUI != null)
            pauseUI.SetActive(false); // Ẩn giao diện Pause khi bắt đầu
        if (musicUI != null)
            musicUI.SetActive(false);// Ẩn giao diện Music khi bắt đầu
        if (runEffect != null)
            runEffect.SetActive(false); // Tắt hiệu ứng chạy khi bắt đầu
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Menu")
        {
            if (pauseUI != null && pauseUI.activeSelf)
            {
                ClosePauseUI(); // Đóng giao diện Pause nếu đang mở
            }
            else
            {
                TogglePause(); // Hiển thị giao diện Pause nếu không mở
            }
        }
    }

    public void TurnOnEffect()
    {
        if (runEffect != null)
        {
            runEffect.SetActive(true); // Bật hiệu ứng chạy
        }

        if (playerController != null)
        {
            playerController.moveSpeed += 1f;
            playerController.maxJumpCount = 2;
        }
    }

    public void TurnOffEffect()
    {

        if (runEffect != null)
        {
            runEffect.SetActive(false); // Tắt hiệu ứng chạy
        }

        if (playerController != null)
        {
            playerController.moveSpeed = 5f;
            playerController.maxJumpCount = 1; // Đặt lại số lần nhảy tối đa về 1
        }
    }

    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            UpdateScore();
        }
    }

    public void AddScoreKey(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            scoreKey += points;
            UpdateScoreKey();
        }
    }

    public void AddLive(int lives)
    {
        if (!isGameOver && !isGameWin)
        {
            countLive += lives;
            UpdateLive();
        }
    }

    private void UpdateScore()
    {
        if (scoreText != null)
            scoreText.text = score.ToString();
    }

    private void UpdateScoreKey()
    {
        if (scoreKeyText != null)
            scoreKeyText.text = scoreKey.ToString();
    }

    private void UpdateLive()
    {
        if (live != null)
            live.text = countLive.ToString();
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        scoreKey = 0;
        Time.timeScale = 0; // Dừng thời gian để tạm dừng trò chơi
        gameOverUI.SetActive(true); // Hiển thị giao diện Game Over
    }
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0; // Dừng thời gian để tạm dừng trò chơi
        gameWinUI.SetActive(true); // hien thị giao diện Game Wins
    }

    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        scoreKey = 0;
        UpdateScore();
        Time.timeScale = 1; // bắt đầu lại trò chơi
        SceneManager.LoadScene("Game");
    }

    public void GotoMenu()
    {
        SceneManager.LoadScene("Menu");
        Time.timeScale = 1; // bắt đầu lại trò chơi
    }

    public void ToggleCartUI()
    {
        if (cartUI != null)
        {
            bool isActive = cartUI.activeSelf;
            cartUI.SetActive(!isActive); // Chuyển đổi trạng thái hiển thị của giỏ hàng
            Time.timeScale = isActive ? 1 : 0; // Dừng hoặc tiếp tục trò chơi
        }
    }

    public void TogglePause()
    {
        if (pauseUI != null)
        {
            bool isPaused = pauseUI.activeSelf;
            pauseUI.SetActive(!isPaused); // Chuyển đổi trạng thái hiển thị của giao diện Pause
            Time.timeScale = isPaused ? 1 : 0; // Dừng hoặc tiếp tục trò chơi
        }
    }

    public void ClosePauseUI()
    {
        if (pauseUI != null && pauseUI.activeSelf)
        {
            pauseUI.SetActive(false); // Ẩn giao diện Pause
            Time.timeScale = 1; // Tiếp tục trò chơi
        }
    }

    public void ResumeGame()
    {
        if (pauseUI != null && pauseUI.activeSelf)
        {
            pauseUI.SetActive(false); // Ẩn giao diện Pause
            Time.timeScale = 1; // Tiếp tục trò chơi
        }
    }

    public void MenuBtn()
    {
        if (pauseUI != null && pauseUI.activeSelf)
        {
            pauseUI.SetActive(false); // Ẩn giao diện Pause

        }
        GotoMenu(); // Trở về menu chính
    }

    public void RestartGameBtn()
    {
        if (pauseUI != null && pauseUI.activeSelf)
        {
            pauseUI.SetActive(false); // Ẩn giao diện Pause
        }
        RestartGame(); // Bắt đầu lại trò chơi
    }


    public void ToggleMusicBtn()
    {
        if (musicUI != null && musicBtn != null)
        {
            musicUI.SetActive(!musicUI.activeSelf); // Chuyển đổi trạng thái hiển thị của nút Music
            ClosePauseUI(); // Đóng giao diện Pause nếu đang mở
            if (musicUI.activeSelf)
            {
                Time.timeScale = 0; // Dừng thời gian khi mở giao diện Music
            }
            else
            {
                Time.timeScale = 1; // Tiếp tục trò chơi khi đóng giao diện Music
            }
        }
    }

    public bool IsGameOver()
    {
        return isGameOver;
    }

    public bool IsGameWin()
    {
        return isGameWin;
    }

}
