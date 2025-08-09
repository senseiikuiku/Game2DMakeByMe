using Unity.VisualScripting.Antlr3.Runtime.Tree;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class SwipeController : MonoBehaviour, IEndDragHandler
{
    [SerializeField] private int maxPage;// Số lượng trang tối đa
    int currentPage;// Trang hiện tại
    Vector3 targetPos;// Vị trí mục tiêu để di chuyển trang
    [SerializeField] private Vector3 pageStep;// Bước di chuyển giữa các trang (page width + spacing)
    [SerializeField] private RectTransform levelPagesRect;// RectTransform của trang cấp độ
    private float dragThreshold; // Ngưỡng kéo để xác định khi nào di chuyển trang

    [SerializeField] private Image[] barImage;// Mảng hình ảnh để hiển thị thanh trạng thái trang hiện tại

    [SerializeField] private float tweenTime; // Thời gian tweening khi di chuyển trang
    [SerializeField] private LeanTweenType tweenType; // Loại tweening để sử dụng

    [SerializeField] private Button previousBtn, nextBtn; // Nút để chuyển đến trang trước và trang tiếp theo

    private void Awake()
    {
        currentPage = 1; // Khởi tạo trang hiện tại là trang đầu tiên
        targetPos = levelPagesRect.localPosition; // Lưu vị trí hiện tại của RectTransform
        dragThreshold = Screen.width / 15; // Thiết lập ngưỡng kéo là 1/15 chiều
        UpdateBar(); // Cập nhật thanh trạng thái ban đầu
        UpdateArrowButton(); // Cập nhật trạng thái nút mũi tên
    }

    public void Next()
    {
        if (currentPage < maxPage)
        {
            currentPage++;// Tăng trang hiện tại
            targetPos += pageStep;// Cập nhật vị trí mục tiêu bằng cách cộng thêm bước di chuyển
            MovePage();// Di chuyển trang đến vị trí mục tiêu
        }
    }

    public void Previous()
    {
        if (currentPage > 1)
        {
            currentPage--;// Giảm trang hiện tại
            targetPos -= pageStep;// Cập nhật vị trí mục tiêu bằng cách trừ bước di chuyển
            MovePage();// Di chuyển trang đến vị trí mục tiêu
        }
    }

    public void MovePage()
    {
        levelPagesRect.LeanMoveLocal(targetPos, tweenTime).setEase(tweenType);// Sử dụng LeanTween để di chuyển trang đến vị trí mục tiêu
        UpdateBar(); // Cập nhật thanh trạng thái sau khi di chuyển trang
        UpdateArrowButton(); // Cập nhật trạng thái nút mũi tên sau khi di chuyển trang
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (Mathf.Abs(eventData.position.x - eventData.pressPosition.x) > dragThreshold)
        {
            // Nếu kéo vượt quá ngưỡng, xác định hướng kéo
            if (eventData.position.x > eventData.pressPosition.x)
            {
                // Kéo sang trái, chuyển đến trang trước
                Previous();
            }
            else
            {
                // Kéo sang phải, chuyển đến trang tiếp theo
                Next();
            }
        }
        else
        {
            // Nếu kéo không đủ xa, giữ nguyên trang hiện tại
            MovePage();
        }
    }

    public void UpdateBar()
    {
        // Cập nhật thanh trạng thái dựa trên trang hiện tại
        for (int i = 0; i < barImage.Length; i++)
        {
            barImage[i].color = new Color(1, 1, 1, 0.55f); // Đặt màu sắc của thanh trạng thái
        }
        barImage[currentPage - 1].color = new Color(1, 1, 1, 1); // Đặt màu sắc của thanh trạng thái cho trang hiện tại
    }

    private void UpdateArrowButton()
    {
        nextBtn.interactable = currentPage < maxPage;
        previousBtn.interactable = currentPage > 1;
    }
}
