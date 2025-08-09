using Unity.VisualScripting;
using UnityEngine;

public class ShowMainMenuUI : MonoBehaviour
{
    public void OnButtonClick()
    {
        GameManager.Instance.GotoMenu(); // Gọi hàm để hiển thị giao diện chính
    }
}
