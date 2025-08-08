using UnityEngine;

public class FireBulletLv2 : FireBulletManager
{
    [SerializeField] private GameObject explosionPrefab; // Hiệu ứng nổ khi viên đạn va chạm

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag("EnemyLv1") ||
            collision.collider.CompareTag("EnemyLv2") ||
            collision.collider.CompareTag("Boss") ||
            collision.collider.CompareTag("MysteryBlock") ||
                collision.gameObject.CompareTag("FireBallEffect") |
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Tạo hiệu ứng nổ
            audioManager.PlayExplosionLv2(); // Phát âm thanh nổ
            Destroy(gameObject); // Phá hủy viên đạn khi va chạm với kẻ thù hoặc mặt đất
        }
    }
}
