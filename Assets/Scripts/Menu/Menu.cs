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
    }

    public void OpenSettings()
    {
        UIManager.Instance.ToggleMusicUI();
    }

    public void QuitGame()
    {
        PlayerPrefs.DeleteKey("KeyRound");
        Application.Quit();
    }

}
