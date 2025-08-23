using UnityEngine.EventSystems;
using UnityEngine;
using System.Collections.Generic;
public class GameObjectPanel : MonoBehaviour
{
    [SerializeField] private GameObject gameObjectPanel;

    void Update()
    {
        // Nếu gameObjectPanel đang hiển thị và người dùng nhấn chuột trái không phải UI element nào
        if (gameObjectPanel.activeSelf && Input.GetMouseButtonDown(0))
        {
            // Kiểm tra xem chuột có đang ở trên một UI element hay không
            if (!IsPointerOverUIElement())
            {
                gameObjectPanel.SetActive(false);
                Time.timeScale = 1; // Tiếp tục trò chơi
            }
        }
    }

    private bool IsPointerOverUIElement()
    {
        // Kiểm tra xem chuột có đang ở trên một UI element hay không
        PointerEventData eventData = new PointerEventData(EventSystem.current);
        // Lấy vị trí chuột và chuyển
        eventData.position = Input.mousePosition;

        // Tạo một danh sách để chứa các kết quả raycast
        var raycastResults = new List<RaycastResult>();
        // Thực hiện raycast để kiểm tra các UI element
        EventSystem.current.RaycastAll(eventData, raycastResults);
        // Nếu có ít nhất một kết quả raycast, nghĩa là chuột đang ở trên một UI element
        return raycastResults.Count > 0;
    }
}
