using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextRoundGate : MonoBehaviour
{
    [SerializeField] private string sceneName;// Tên của round tiếp theo
    public int roundNumber; // Số vòng chơi


    private void LoadRoundScene()
    {
        if (GameManager.Instance != null && GameManager.Instance.keyRound >= roundNumber)
        {
            GameManager.Instance.scoreKey = 0;
            GameManager.Instance.ResetGameFlags(); // Reset lại gameWin và gameOver
            Time.timeScale = 1;// Trò chơi sẽ tiếp tục
            SceneManager.LoadScene(sceneName);
        }
    }

    public void OnButtonClick()
    {
        LoadRoundScene();
    }

}
