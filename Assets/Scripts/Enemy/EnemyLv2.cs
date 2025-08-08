using UnityEngine;

public class EnemyLv2 : EnemyManager
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            string enemyLvDie = "isEnemyLv2Die";

            ContactPoint2D contact = collision.contacts[0]; // Lấy điểm va chạm đầu tiên

            HandlePlayerCollision(contact, enemyLvDie); // Gọi hàm xử lý va chạm với người chơi
        }
        else if (collision.gameObject.CompareTag("BulletLv1"))
        {
            Rigidbody2D bulletRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của viên đạn
            if (bulletRb != null)
            {
                Vector2 bulletDir = bulletRb.linearVelocity.normalized; // Lấy hướng của viên đạn
                EnemyLv2DieByBullet(bulletDir); // Gọi hàm xử lý khi kẻ thù bị bắn
            }
            Destroy(collision.gameObject); // Xóa viên đạn khi va chạm với kẻ thù
        }
        else if (collision.gameObject.CompareTag("BulletLv2"))
        {
            Rigidbody2D bulletRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của viên đạn
            if (bulletRb != null)
            {
                Vector2 bulletDir = bulletRb.linearVelocity.normalized; // Lấy hướng của viên đạn
                EnemyLv2DieByBullet(bulletDir); // Gọi hàm xử lý khi kẻ thù bị bắn
            }
            // Không cần Destroy đạn ở đây vì nó tự nổ trong FireBulletLv2
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("SlashingLv1") || collision.gameObject.CompareTag("SlashingLv2"))
        {
            // Xử lý khi kẻ thù bị chém bởi SlashingLv1
            Rigidbody2D slashingRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của SlashingLv1 hoặc SlashingLv2
            if (slashingRb != null)
            {
                Vector2 slashDir = slashingRb.linearVelocity.normalized; // Lấy hướng của SlashingLv1 hoặc SlashingLv2
                EnemyLv2DieBySlash(slashDir); // Gọi hàm xử lý khi kẻ thù bị chém
            }
        }
        else if (collision.gameObject.CompareTag("SwordWave"))
        {
            Rigidbody2D swordWaveRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của SwordWave
            if (swordWaveRb != null)
            {
                Vector2 swordWaveDir = swordWaveRb.linearVelocity.normalized; // Lấy hướng của SwordWave
                EnemyLv2DieBySwordWave(swordWaveDir); // Gọi hàm xử lý khi kẻ thù bị chém bởi SwordWave
            }
            Destroy(collision.gameObject); // Xóa SwordWave khi va chạm với kẻ thù  
        }
    }
}
