using UnityEngine;

public class EnemyLv1 : EnemyManager
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("Player"))
        {
            string enemyLvDie = "isEnemyDie";
            ContactPoint2D contact = collision.GetContact(0); // Lấy điểm va chạm đầu tiên giữa kẻ thù và người chơi
            HandlePlayerCollision(contact, enemyLvDie); // Gọi hàm xử lý va chạm với người chơi

        }
        else if (collision.gameObject.CompareTag("BulletLv1"))
        {
            Rigidbody2D bulletRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của viên đạn
            if (bulletRb != null)
            {
                Vector2 bulletDir = bulletRb.linearVelocity.normalized; // Lấy hướng của viên đạn
                EnemyLv1DieByBullet(bulletDir); // Gọi hàm xử lý khi kẻ thù bị bắn
            }
            Destroy(collision.gameObject); // Xóa viên đạn khi va chạm với kẻ thù
        }
        else if (collision.gameObject.CompareTag("BulletLv2"))
        {
            Rigidbody2D bulletRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của viên đạn
            if (bulletRb != null)
            {
                Vector2 bulletDir = bulletRb.linearVelocity.normalized; // Lấy hướng của viên đạn
                EnemyLv1DieByBullet(bulletDir); // Gọi hàm xử lý khi kẻ thù bị bắn
            }
            // Không cần Destroy đạn ở đây vì nó tự nổ trong FireBulletLv2
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.gameObject.CompareTag("SlashingLv1") || collision.gameObject.CompareTag("SlashingLv2"))
        {
            Rigidbody2D slashRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (slashRb != null)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized; // Lấy hướng từ đòn chém đến kẻ thù
                EnemyLv1DieBySlash(direction);
            }
        }
        else if (collision.gameObject.CompareTag("SwordWave"))
        {
            // Xử lý khi kẻ thù bị chém bởi sóng kiếm
            Rigidbody2D swordWaveRb = collision.gameObject.GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của sóng kiếm
            if (swordWaveRb != null)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized; // Lấy hướng từ đòn chém đến kẻ thù
                EnemyLv1DieBySwordWave(direction); // Gọi hàm xử lý khi kẻ thù bị chém
            }
            Destroy(collision.gameObject); // Xóa sóng kiếm khi va chạm với kẻ thù
        }

    }

}
