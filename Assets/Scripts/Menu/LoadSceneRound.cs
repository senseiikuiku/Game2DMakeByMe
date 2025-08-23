using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
public class LoadSceneRound : MonoBehaviour
{
    [SerializeField] private Sprite[] courseImg; // Mảng hình ảnh khóa hoặc mở khóa
    private Image img; // Biến để lưu trữ hình ảnh
    Button btn; // Biến để lưu trữ nút
    [SerializeField] private string sceneName; // Tên của scene cần tải
    [SerializeField] private int roundNumber; // Số vòng chơi

    private void Awake()
    {
        if (GameManager.Instance == null)
        {
            Debug.LogError("GameManager chưa được khởi tạo!");
            return;
        }
        img = gameObject.GetComponent<Image>(); // Lấy thành phần Image từ GameObject này
        btn = gameObject.GetComponent<Button>(); // Lấy thành phần Button từ GameObject này
        UpdateKeyRound(); // Cập nhật trạng thái nút dựa trên số chìa khóa hiện tại
        btn.onClick.AddListener(LoadRoundScene); // Gọi hàm để tải scene khi khởi tạo
    }

    public void LoadRoundScene()
    {
        if (GameManager.Instance != null && GameManager.Instance.keyRound >= roundNumber)
        {
            btn.interactable = true; // Kích hoạt nút nếu đủ chìa khóa
            Time.timeScale = 1; // Đảm bảo thời gian trò chơi được tiếp tục
            SceneManager.LoadScene(sceneName); // Tải scene mới
        }
        else if (GameManager.Instance.keyRound < roundNumber)
        {
        }
    }

    public void UpdateKeyRound()
    {
        if (GameManager.Instance != null && GameManager.Instance.keyRound < roundNumber)
        {
            img.sprite = courseImg[1]; // Chọn hình ảnh khóa nếu không đủ chìa khóa
            btn.interactable = false; // Vô hiệu hóa nút nếu không đủ chìa khóa
        }
        else
        {
            img.sprite = courseImg[0]; // Chọn hình ảnh mở khóa nếu đủ chìa khóa
            btn.interactable = true; // Kích hoạt nút nếu đủ chìa khóa
        }
    }
}
