using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;
using System.Collections;
public class GameManager : MonoBehaviour
{
    public int score = 0;
    public int scoreKey = 0; //Theo dõi số lượng chìa khóa đã thu thập

    public int countLive = 3; // Số mạng mà người chơi có

    private bool isGameOver = false;

    private bool isGameWin = false;

    private PlayerController playerController; // Tham chiếu đến PlayerController để xử lý các hành động của người chơi



    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        if (scene.name == "Game")
        {
            FindPlayerController();
        }
    }

    private void FindPlayerController()
    {

        playerController = FindAnyObjectByType<PlayerController>();
        playerController.enabled = true;
        if (playerController == null)
        {
            Debug.LogWarning("Không tìm thấy PlayerController trong scene!");
        }
        else
        {
            Debug.Log("Đã tìm thấy PlayerController mới!");
        }
    }


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
        SceneManager.sceneLoaded += OnSceneLoaded; // Đăng ký sự kiện khi cảnh được tải

    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && SceneManager.GetActiveScene().name != "Menu")
        {
            UIManager.Instance.TogglePauseUI();
        }
    }

    public void TurnOnEffect()
    {
        UIManager.Instance.ToggleRunEffect(true);

        if (playerController != null)
        {
            playerController.maxJumpCount = 2;
            playerController.moveSpeed += 1f;
        }
    }

    public void TurnOffEffect()
    {
        UIManager.Instance.ToggleRunEffect(false);

        if (playerController != null)
        {
            playerController.moveSpeed = 5f;
        }
    }

    public void AddScore(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            score += points;
            UIManager.Instance.UpdateScore(score);
        }
    }

    public void AddScoreKey(int points)
    {
        if (!isGameOver && !isGameWin)
        {
            scoreKey += points;
            UIManager.Instance.UpdateScoreKey(scoreKey);
        }
    }

    public void AddLive(int lives)
    {
        if (!isGameOver && !isGameWin)
        {
            countLive += lives;
            UIManager.Instance.UpdateLive(countLive);
        }
    }

    public void GameOver()
    {
        isGameOver = true;
        score = 0;
        scoreKey = 0;
        Time.timeScale = 0;
        UIManager.Instance.ShowGameOverUI();
    }
    public void GameWin()
    {
        isGameWin = true;
        Time.timeScale = 0;
        UIManager.Instance.ShowGameWinUI();
    }

    public void RestartGame()
    {
        isGameOver = false;
        score = 0;
        scoreKey = 0;
        countLive = 3;
        Time.timeScale = 1;
        SceneManager.LoadScene("Game");
    }

    public void GotoMenu()
    {
        ResetGameFlags();
        score = 0;
        scoreKey = 0;
        countLive = 3;
        Time.timeScale = 1;
        SceneManager.LoadScene("Menu");
    }

    public void ResetGameFlags()
    {
        isGameOver = false;
        isGameWin = false;
    }


    public bool IsGameOver() => isGameOver;
    public bool IsGameWin() => isGameWin;

}
