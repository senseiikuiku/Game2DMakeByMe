using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();

    }

    public void PlayGame()
    {
        SceneManager.LoadScene("Game");
    }

    public void OpenSettings()
    {
        gameManager.ToggleMusicBtn();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
