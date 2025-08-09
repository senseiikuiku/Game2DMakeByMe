using UnityEngine;
using UnityEngine.SceneManagement;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject scrollViewUI;

    private void Awake()
    {
        if (scrollViewUI != null)
        {
            scrollViewUI.SetActive(false); // Ẩn giao diện ScrollView khi khởi tạo
        }
    }

    public void PlayGame()
    {
        bool isActive = scrollViewUI.activeSelf;
        scrollViewUI.SetActive(!isActive); // Chuyển đổi trạng thái hiển thị của ScrollViewUI
        Time.timeScale = isActive ? 1 : 0; // Dừng hoặc tiếp tục trò chơi
    }

    public void OpenSettings()
    {
        UIManager.Instance.ToggleMusicUI();
    }

    public void QuitGame()
    {
        Application.Quit();
    }

}
