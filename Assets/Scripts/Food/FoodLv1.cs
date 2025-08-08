using UnityEngine;

public class FoodLv1 : MonoBehaviour
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
            Debug.Log("Player collided with FoodLv1");
            audioManager.PlayEatFoodLv1(); // Phát âm thanh khi ăn thức ăn cấp 1
            gameManager.AddLive(1); // Thêm 1 mạng cho người chơi
            Destroy(gameObject); // Phá hủy đối tượng thức ăn
        }
    }

}
