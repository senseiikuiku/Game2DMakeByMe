using UnityEngine;

public class SkillManager : MonoBehaviour
{
    [SerializeField] private GameObject fireBulletPrefabs; // Prefab của viên đạn lửa
    [SerializeField] private GameObject slashPrefabs; // Prefab của đòn chém

    private PlayerController playerController; // Tham chiếu đến PlayerController để tương tác với người chơi
    private AudioManager audioManager; // Tham chiếu đến AudioManager để phát âm thanh

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>(); // Tìm PlayerController trong cảnh
        audioManager = FindAnyObjectByType<AudioManager>(); // Tìm AudioManager trong cảnh
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (playerController == null) return;

        // Nếu chạm vào fireBullet
        if (collision.CompareTag("SkillFireBullet"))
        {
            audioManager.PlayPickUpSkill(); // Phát âm thanh khi nhận được kỹ năng
            Debug.Log("Nhận được FireBullet"); // In ra log để kiểm tra
            playerController.skill = 1; // 1: bắn
            Debug.Log("Đặt skill thành 1 (bắn)"); // In ra log để kiểm tra
            playerController.bulletLevel += 1; // Đặt level bắn đạn (có thể điều chỉnh theo logic game)
            Debug.Log("Tăng level bắn lên: " + playerController.bulletLevel); // In ra log để kiểm tra
            playerController.slashLevel = 0; // Đặt level chém về 0 khi nhận được đạn
            Debug.Log("Đặt level chém về 0"); // In ra log để kiểm tra
            Destroy(collision.gameObject);
        }
        // Nếu chạm vào slash
        else if (collision.CompareTag("SkillSlashing"))
        {
            audioManager.PlayPickUpSkill(); // Phát âm thanh khi nhận được kỹ năng
            Debug.Log("Nhận được Slash"); // In ra log để kiểm tra
            playerController.skill = 2; // 2: chém
            Debug.Log("Đặt skill thành 2 (chém)"); // In ra log để kiểm tra
            playerController.slashLevel += 1; // Đặt level chém (có thể điều chỉnh theo logic game)
            Debug.Log("Tăng level chém lên: " + playerController.slashLevel); // In ra log để kiểm tra
            playerController.bulletLevel = 0; // Đặt level bắn về 0 khi nhận được đòn chém
            Debug.Log("Đặt level bắn về 0"); // In ra log để kiểm tra
            Destroy(collision.gameObject);
        }
    }
}
