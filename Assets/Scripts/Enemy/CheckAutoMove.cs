using UnityEngine;

public class CheckAutoMove : MonoBehaviour
{
    public bool isAutoMove = false; // Biến kiểm tra trạng thái tự động di chuyển
    private PlayerController playerController;

    private void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            // Kiểm tra va chạm với Player
            if (playerController != null)
            {
                // Nếu Player đang ở trạng thái tự động di chuyển, bật chế độ tự động di chuyển cho đối tượng này
                isAutoMove = true;
            }
        }
    }


}
