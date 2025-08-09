using Unity.VisualScripting;
using UnityEngine;

public class ClosedBtn : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectPanel;
    void Start()
    {

    }

    void Update()
    {

    }

    public void ClosePanelUI()
    {
        if (gameObjectPanel != null && gameObjectPanel.activeSelf)
        {
            Time.timeScale = 1; // Tiếp tục trò chơi
            gameObjectPanel.SetActive(false); // Ẩn giao diện Pause
        }
    }

    public void OnButtonClick()
    {
        ClosePanelUI();
    }
}
