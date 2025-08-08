using UnityEngine;

public class FireBulletLv1 : FireBulletManager
{
    [SerializeField] private float bounceForce = 5f;  // Lực nảy lên

    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Nảy lên khi chạm đất
        if (collision.gameObject.layer == LayerMask.NameToLayer("Ground"))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, bounceForce);
        }
        else if (collision.gameObject.CompareTag("FireBallEffect") || collision.gameObject.CompareTag("Boss"))
        {
            Destroy(gameObject); // Phá hủy viên đạn khi va chạm với đạn củaBoss hoặc Boss
        }
    }


}
