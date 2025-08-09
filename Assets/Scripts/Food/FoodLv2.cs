using UnityEngine;

public class FoodLv2 : MonoBehaviour
{
    private AudioManager audioManager;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (GameManager.Instance != null)
            {
                audioManager.PlayEatFoodLv2(); // Phát âm thanh khi ăn thức ăn cấp 2
                GameManager.Instance.AddLive(2);          // Thêm 2 mạng cho người chơi
                GameManager.Instance.TurnOnEffect();      // Bật hiệu ứng khi ăn thức ăn
            }
            Destroy(gameObject); // Phá hủy đối tượng thức ăn
        }
    }
}
