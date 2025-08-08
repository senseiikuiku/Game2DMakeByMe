using UnityEngine;

public class BossFireBullet : FireBulletManager
{

    [SerializeField] private GameObject explosionPrefab; // Hiệu ứng nổ khi viên đạn va chạm

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (
            collision.gameObject.CompareTag("SlashingLv1") ||
            collision.gameObject.CompareTag("SlashingLv2") ||
            collision.gameObject.CompareTag("SwordWave")
            )
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Tạo hiệu ứng nổ
            audioManager.PlayExplosionBoss(); // Phát âm thanh nổ
            Destroy(gameObject); // Phá hủy viên đạn khi va chạm với kẻ thù hoặc mặt đất
        }

    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") ||
            collision.gameObject.CompareTag("MysteryBlock") ||
            collision.gameObject.CompareTag("Trap") ||
            collision.gameObject.CompareTag("BulletLv1") ||
            collision.gameObject.CompareTag("BulletLv2") ||
            collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            Instantiate(explosionPrefab, transform.position, Quaternion.identity); // Tạo hiệu ứng nổ
            audioManager.PlayExplosionBoss(); // Phát âm thanh nổ
            Destroy(gameObject); // Phá hủy viên đạn khi va chạm với kẻ thù hoặc mặt đất
        }
    }


}
