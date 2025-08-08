using UnityEngine;

public abstract class EnemyManager : MonoBehaviour
{
    [SerializeField] protected float speed = 2f; // Tốc độ di chuyển của kẻ thù
    [SerializeField] protected float distance = 5f; // Khoảng cách kẻ địch sẽ di chuyển tới lui
    protected Vector3 startPos; // Vị trí bắt đầu của kẻ thù
    protected bool movingRight = true; // Hướng chuyển động
    protected Animator animator; // Thành phần hoạt hình cho hoạt ảnh (nếu cần)
    protected bool isDead = false; // Để kiểm tra xem kẻ địch đã chết chưa
    protected int lives = 2; // Số mạng của kẻ địch, có thể thay đổi tùy theo yêu cầu


    protected PlayerController playerController; // Tham chiếu đến PlayerController để tương tác với người chơi
    protected GameManager gameManager; // Tham chiếu đến GameManager để quản lý trò chơi
    protected AudioManager audioManager; // Tham chiếu đến AudioManager để quản lý âm thanh

    protected virtual void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>(); // Tìm PlayerController trong cảnh
        animator = GetComponent<Animator>();
        gameManager = FindAnyObjectByType<GameManager>(); // Tìm GameManager trong cảnh
        audioManager = FindAnyObjectByType<AudioManager>(); // Tìm AudioManager trong cảnh
    }

    protected virtual void Start()
    {
        startPos = transform.position;
    }

    protected virtual void Update()
    {
        if (isDead) return; // Nếu kẻ thù đã chết, không thực hiện di chuyển
        float leftBound = startPos.x - distance;
        float rightBound = startPos.x + distance;
        if (movingRight)
        {
            transform.Translate(Vector3.right * speed * Time.deltaTime);
            if (transform.position.x >= rightBound)
            {
                movingRight = false; // Phương hướng chuyển động sẽ đổi khi chạm vào ranh giới bên phải
                Flip(); // Lật mặt của kẻ địch sang mặt bên trái
            }
        }
        else
        {
            transform.Translate(Vector3.left * speed * Time.deltaTime);
            if (transform.position.x <= leftBound)
            {
                movingRight = true; // Phương hướng chuyển động sẽ đổi khi chạm vào ranh giới bên trái
                Flip(); // Lật mặt của kẻ địch sang mặt bên phải
            }
        }
    }

    protected void Flip()
    {
        Vector3 scale = transform.localScale;
        scale.x *= -1; // Lật theo trục x
        transform.localScale = scale; // Áp dụng thang đo lật
    }

    // Hàm này sẽ được gọi khi kẻ thù va chạm với người chơi
    protected void HandlePlayerCollision(ContactPoint2D contact, string enemyLvDie)
    {
        bool stomped = contact.normal.y < -0.5f;
        bool hitPlayerSide = contact.normal.y >= -0.5f;

        if (stomped)
        {
            HandleDeath(enemyLvDie);
        }
        else if (hitPlayerSide)
        {
            if (playerController != null && !playerController.isInvincible)
            {
                playerController.TakeDamage(transform.position);
            }
        }
    }


    // Hàm này sẽ được gọi khi kẻ thù bị người chơi tấn công
    protected void HandleDeath(string enemyLvDie)
    {
        isDead = true;
        animator.SetBool(enemyLvDie, true);
        speed = 0;

        if (playerController != null)
        {
            playerController.rb.linearVelocity = new Vector2(playerController.rb.linearVelocity.x, 10f); // Đẩy người chơi lên khi kẻ thù chết
        }

        audioManager.PlayEnemyDieSound();

        Destroy(gameObject, 0.5f);
    }

    // Hàm này sẽ được gọi để đặt lại hoạt ảnh khi kẻ thù bị tấn công
    protected void ResetEnemyHitAnimation()
    {
        animator.SetBool("isEnemyLv2Die", false);
    }

    // Hàm này sẽ được gọi khi kẻ thù bị nâng lên trên bởi kỹ năng của người chơi
    protected void EnemyLiftedUp(Vector2 skillDirection, float directionX, float directionY)
    {
        // Văng lên trên nhẹ
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của kẻ thù
        if (rb != null)
        {
            rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Giữ nguyên hướng xoay của kẻ thù
            rb.linearVelocity = new Vector2(skillDirection.x * directionX, directionY); // văng lên và sang ngang
            rb.gravityScale = 1.5f; // Tăng trọng lực để rơi nhanh hơn
            rb.constraints = RigidbodyConstraints2D.None; // Bỏ ràng buộc để kẻ thù có thể rơi tự do, Thả tự do để xoay
            audioManager.PlayEnemyDieSound(); // Phát âm thanh khi kẻ thù chết
            rb.constraints = RigidbodyConstraints2D.None; // Bỏ ràng buộc để kẻ thù có thể rơi tự do, Thả tự do để xoay
        }
    }

    // Hàm này sẽ được gọi khi kẻ thù bị giết bởi kỹ năng của người chơi
    protected void EnemyDieBySkill(Vector2 skillDirection, float directionX, float directionY, string enemyLvDie)
    {
        isDead = true;
        speed = 0;

        // Văng lên trên nhẹ
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của kẻ thù
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(skillDirection.x * directionX, directionY); // văng lên và sang ngang
            rb.gravityScale = 1.5f; // Tăng trọng lực để rơi nhanh hơn
            rb.constraints = RigidbodyConstraints2D.None; // Bỏ ràng buộc để kẻ thù có thể rơi tự do, Thả tự do để xoay
            rb.AddTorque(50f); // Quay ngang 
        }

        animator.SetBool(enemyLvDie, true);
        audioManager.PlayEnemyDieSound(); // Phát âm thanh khi kẻ thù chết
        Destroy(gameObject, 0.5f); // Xóa kẻ thù sau 0.5 giây
    }

    // Hàm này sẽ được gọi khi kẻ thù lv1 bị bắn bởi đạn
    protected void EnemyLv1DieByBullet(Vector2 bulletDirection)
    {
        if (isDead) { return; }

        EnemyDieBySkill(bulletDirection, 3f, 3f, "isEnemyDie"); // Gọi hàm xử lý khi kẻ thù bị bắn

    }

    // Hàm này sẽ được gọi khi kẻ thù lv1 bị chém bởi slash
    protected void EnemyLv1DieBySlash(Vector2 slashDirection)
    {
        if (isDead) { return; }

        EnemyDieBySkill(slashDirection, 2f, 3f, "isEnemyDie"); // Gọi hàm xử lý khi kẻ thù bị chém
    }

    // Hàm này sẽ được gọi khi kẻ thù lv1 bị chém bởi sóng kiếm
    protected void EnemyLv1DieBySwordWave(Vector2 swordWaveDirection)
    {
        if (isDead) { return; }

        EnemyDieBySkill(swordWaveDirection, 4f, 4f, "isEnemyDie"); // Gọi hàm xử lý khi kẻ thù bị chém bởi sóng kiếm
    }

    // Hàm này sẽ được gọi khi kẻ thù lv2 bị bắn bởi đạn
    protected void EnemyLv2DieByBullet(Vector2 bulletDirection)
    {
        if (isDead) { return; }

        if (playerController.bulletLevel == 1 && playerController.skill == 1)
        {
            lives--; // Giảm 1 mạng khi bị bắn bằng đạn cấp 1
            if (lives == 1)
            {
                animator.SetBool("isEnemyLv2Die", true); // Bật hoạt ảnh chết
                EnemyLiftedUp(bulletDirection, 3f, 3f); // Văng lên trên nhẹ
                Invoke(nameof(ResetEnemyHitAnimation), 0.5f); // Đặt lại hoạt ảnh sau 0.5 giây
            }
            else if (lives <= 0)
            {
                EnemyDieBySkill(bulletDirection, 3f, 3f, "isEnemyLv2Die"); // Gọi hàm xử lý khi kẻ thù chết
            }
        }
        else if (playerController.bulletLevel >= 2 && playerController.skill == 1)
        {
            lives = 0; // Giảm mạng xuống 0 khi bị bắn bằng đạn cấp 2
            if (lives <= 0)
            {
                EnemyDieBySkill(bulletDirection, 3f, 3f, "isEnemyLv2Die"); // Gọi hàm xử lý khi kẻ thù chết
            }
        }
    }

    // Hàm này sẽ được gọi khi kẻ thù lv2 bị chém bởi slash
    protected void EnemyLv2DieBySlash(Vector2 slashDirection)
    {
        if (isDead) { return; }

        if (playerController.slashLevel == 1 && playerController.skill == 2)
        {
            lives--; // Giảm 1 mạng khi bị chém bằng slash cấp 1
            if (lives == 1)
            {
                animator.SetBool("isEnemyLv2Die", true); // Bật hoạt ảnh chết
                EnemyLiftedUp(slashDirection, 3f, 3f); // Văng lên trên nhẹ
                Invoke(nameof(ResetEnemyHitAnimation), 0.5f); // Đặt lại hoạt ảnh sau 0.5 giây
            }
            else if (lives <= 0)
            {
                EnemyDieBySkill(slashDirection, 3f, 3f, "isEnemyLv2Die"); // Gọi hàm xử lý khi kẻ thù chết
            }
        }
        else if (playerController.slashLevel >= 2 && playerController.skill == 2)
        {
            lives = 0; // Giảm mạng xuống 0 khi bị chém bằng slash cấp 2
            if (lives <= 0)
            {
                EnemyDieBySkill(slashDirection, 3f, 3f, "isEnemyLv2Die"); // Gọi hàm xử lý khi kẻ thù chết
            }
        }
    }

    // Hàm này sẽ được gọi khi kẻ thù lv2 bị chém bởi sóng kiếm
    protected void EnemyLv2DieBySwordWave(Vector2 swordWaveDirection)
    {
        if (isDead) { return; }

        lives = 0;
        if (lives <= 0)
        {
            EnemyDieBySkill(swordWaveDirection, 4f, 4f, "isEnemyLv2Die"); // Gọi hàm xử lý khi kẻ thù chết
        }
    }
}
