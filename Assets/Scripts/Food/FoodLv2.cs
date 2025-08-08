using UnityEngine;

public class FoodLv2 : MonoBehaviour
{
    private GameManager gameManager;
    private AudioManager audioManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (gameManager != null)
            {
                audioManager.PlayEatFoodLv2(); // Phát âm thanh khi ăn thức ăn cấp 2
                gameManager.AddLive(2);          // Thêm 2 mạng cho người chơi
                gameManager.TurnOnEffect();      // Bật hiệu ứng khi ăn thức ăn
            }
            Destroy(gameObject); // Phá hủy đối tượng thức ăn
        }
    }
}
