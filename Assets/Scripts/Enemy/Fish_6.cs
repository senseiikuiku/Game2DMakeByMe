using UnityEngine;

public class Fish_6 : EnemyAutoManager
{
    protected override void Update()
    {
        if (playerController != null && playerController.isInWater)
        {
            base.Update(); // Chỉ gọi Update của lớp EnemyAutoManager khi người chơi ở trong nước
        }
        else
        {
            animator.SetBool("isRunning", false); // Tắt hoạt ảnh chạy nếu người chơi không ở trong nước
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (isDead) return;
        string tag = collision.gameObject.tag;

        if (tag == "Player")
        {
            string nameEnemy = gameObject.name;
            ContactPoint2D contact = collision.GetContact(0);
            HandlePlayerCollision(contact, "isDead", nameEnemy);
        }
        else if (tag == "BulletLv1" || tag == "BulletLv2")
        {
            Rigidbody2D bulletRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (bulletRb != null)
            {
                Vector2 bulletDir = bulletRb.linearVelocity.normalized; // Lấy hướng của viên đạn
                EnemySlimeDieByBullet(bulletDir);
            }

            if (tag == "BulletLv1")
                Destroy(collision.gameObject); // Lv1 biến mất ngay
            // Lv2 tự hủy, không cần Destroy
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isDead) return;

        if (collision.CompareTag("SlashingLv1") || collision.CompareTag("SlashingLv2"))
        {
            Rigidbody2D slashRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (slashRb != null)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized;// Lấy hướng của đòn chém đến kẻ thù
                EnemySlimeDieBySlash(direction);
            }
        }
        else if (collision.CompareTag("SwordWave"))
        {
            Rigidbody2D swordWaveRb = collision.gameObject.GetComponent<Rigidbody2D>();
            if (swordWaveRb != null)
            {
                Vector2 direction = (transform.position - collision.transform.position).normalized; // Lấy hướng của sóng kiếm đến kẻ thù
                EnemySlimeDieBySwordWave(direction);
            }
            Destroy(collision.gameObject); // Xóa sóng kiếm khi va chạm với kẻ thù
        }
    }
}
