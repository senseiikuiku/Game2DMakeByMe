using System.Collections;
using UnityEngine;
using static UnityEditor.Rendering.ShadowCascadeGUI;

public class PlayerController : MonoBehaviour
{
    public float moveSpeed = 5f; // Tốc độ di chuyển của người chơi
    [SerializeField] public float jumpForce = 18f; // Lực tác dụng khi nhảy
    [SerializeField] private LayerMask groundLayer; // Lớp để kiểm tra va chạm mặt đất
    [SerializeField] private Transform groundCheck; // Biến để kiểm tra xem người chơi có ở trên mặt đất không
    private bool isGrounded; // Cờ để kiểm tra xem người chơi có ở trên mặt đất không
    private Animator animator; // Thành phần hoạt hình cho hoạt ảnh (nếu cần)
    public Rigidbody2D rb;
    private Player_UI playerUI; // Tham chiếu đến Player_UI để cập nhật giao diện người chơi
    private AudioManager audioManager; // Tham chiếu đến AudioManager để biết hiệu ứng âm thanh
    private PlayerCollision playerCollision; // Tham chiếu đến PlayerCollision để xử lý va chạm với kẻ địch hoặc cạm bẫy

    public bool isInvincible = false; // Bất tử tạm thời
    private SpriteRenderer spriteRenderer;
    private bool isStunned = false; // Không điều khiển được tạm thời

    public int bulletLevel = 0; // Cấp độ bắn đạn, có thể mở rộng sau này nếu cần

    [SerializeField] private GameObject bulletLv1Prefab; // đang cấp bắn đạn cấp 1
    [SerializeField] private float angleBulletLlv1 = 30f; // Góc bắn đạn

    [SerializeField] private GameObject bulletLv2Prefab; // Đạn cấp 2 (nếu có, có thể mở rộng sau này)


    [SerializeField] private Transform bulletSpawnPoint; // Vị trí bắn đạn

    [SerializeField] private float fireCooldown = 0.5f; // Thời gian chờ giữa các lần bắn
    private float lastFireTime = -999f; // Thời gian của lần bắn cuối cùng

    public int skill = 0; // 1. bắn , 2. chém, 3. ...
    [SerializeField] private SlashComboLv1 slashComboLv1;
    [SerializeField] private SlashComboLv2 slashComboLv2;
    public int slashLevel = 0; // Cấp độ chém, có thể mở rộng sau này nếu cần


    public void Awake()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        playerUI = FindAnyObjectByType<Player_UI>(); // Tìm Player_UI trong cảnh
        audioManager = FindAnyObjectByType<AudioManager>();
        playerCollision = FindAnyObjectByType<PlayerCollision>(); // Tìm PlayerCollision trong cảnh
        spriteRenderer = GetComponent<SpriteRenderer>(); // Lấy SpriteRenderer để thay đổi màu sắc khi bất tử
    }
    void Start()
    {
    }

    void Update()
    {
        if (isStunned)
            return;
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWin())
            return; // Nếu trò chơi đã kết thúc hoặc thắng, không xử lý điều khiển
        HandleMovement();
        HandleJump();
        HandleSeat();
        UpdateAnimation();
        HandleSkill(); // Xử lý kỹ năng bắn hoặc chém

    }


    public int facingDirection = 1; // 1: phải, -1: trái
    private void HandleMovement()
    {
        float moveInput = Input.GetAxis("Horizontal");
        rb.linearVelocity = new Vector2(moveInput * moveSpeed, rb.linearVelocity.y);
        if (moveInput > 0)
        {
            transform.localScale = new Vector3(1, 1, 1); // lật mặt phải
            facingDirection = 1; // Đặt hướng đối diện là phải
        }
        else if (moveInput < 0)
        {
            transform.localScale = new Vector3(-1, 1, 1); // laật mặt trái
            facingDirection = -1; // Đặt hướng đối diện là trái
        }
    }

    private int jumpCount = 0;
    public int maxJumpCount = 1;
    private bool wasGrounded;// Biến để lưu trạng thái mặt đất trước đó

    private void HandleJump()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, 0.2f, groundLayer); // Kiểm tra xem người chơi có đang ở trên mặt đất hay không
        if (isGrounded && !wasGrounded)
        {
            jumpCount = maxJumpCount;
        }
        if (Input.GetKeyDown(KeyCode.UpArrow) && jumpCount > 0)
        {
            audioManager.PlayJumpSound(); // Hiệu ứng âm thanh khi nhảy
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount--;
        }
        wasGrounded = isGrounded;
    }

    private void UpdateAnimation()
    {
        bool isRunning = Mathf.Abs(rb.linearVelocity.x) > 0.1f; // kiểm tra xem người chơi có đang di chuyển hay không
        bool isJumping = !isGrounded;
        bool isSitting = isGrounded && Input.GetKey(KeyCode.DownArrow);

        // Ưu tiên isSitting nếu đang ngồi
        animator.SetBool("isRunning", isRunning && !isSitting);
        animator.SetBool("isJumping", isJumping);
        animator.SetBool("isSitting", isSitting);

        // Player_UI
        playerUI.animator.SetBool("isRunning", isRunning && !isSitting);
        playerUI.animator.SetBool("isSitting", isSitting);
    }

    private void HandleSeat()
    {
        if (isGrounded && Input.GetKey(KeyCode.DownArrow))
        {
            animator.SetBool("isSitting", true);
            playerUI.animator.SetBool("isSitting", true);
        }
        else
        {
            animator.SetBool("isSitting", false);
            playerUI.animator.SetBool("isSitting", false);
        }
    }

    // Phương thức này sẽ được gọi khi người chơi bị tấn công bởi kẻ địch hoặc cạm bẫy
    public void TakeDamage(Vector2 sourcePosition)
    {
        if (isInvincible)
            return;

        GameManager.Instance.AddLive(-1); // Giảm số mạng của người chơi khi bị tấn công

        if (GameManager.Instance != null)
        {
            GameManager.Instance.TurnOffEffect(); // Tắt hiệu ứng nếu không có GameManager   
        }

        playerCollision.ReduceSkillLevel(); // Giảm cấp độ kỹ năng khi bị tấn công

        // Knockback
        Vector2 knockbackDir = ((Vector2)transform.position - sourcePosition).normalized;
        float knockbackForce = 2f;
        rb.linearVelocity = new Vector2(knockbackDir.x * knockbackForce, 4f);
        // Bất tử và khựng
        StartCoroutine(BecomeInvincible(1f));
        audioManager.PlayEnemyOrTrapHitSound(); // Người chơi bị Enemy tấn công hoặc dính cạm bẫy

        if (GameManager.Instance != null && GameManager.Instance.countLive <= 0)
        {
            StartCoroutine(HandleDeathAndGameOver()); // Gọi hàm xử lý chết và game over

        }
    }


    private IEnumerator HandleDeathAndGameOver()
    {
        playerUI.animator.SetTrigger("Dead"); // Hành động animation chết trên Player_UI
        animator.SetTrigger("Dead"); // hành động animation chết
        yield return new WaitForSeconds(1f); // Chờ animation chết hoàn thành (có thể điều chỉnh thời gian)
        audioManager.PlayGameOverSound(); // Phát âm thanh game over
        GameManager.Instance.GameOver();
    }


    // Thay thế logic nhấp nháy trong BecomeInvincible bằng màu đỏ nhấp nháy
    public IEnumerator BecomeInvincible(float duration)
    {
        isInvincible = true;
        isStunned = true;

        float flashDelay = 0.1f;
        float timer = 0f;
        Color originalColor = spriteRenderer.color;
        Color flashColor = Color.red;

        while (timer < duration)
        {
            spriteRenderer.color = flashColor; // Thay đổi màu sắc thành đỏ
            yield return new WaitForSeconds(flashDelay);

            spriteRenderer.color = originalColor; // Khôi phục màu gốc
            yield return new WaitForSeconds(flashDelay);
            timer += flashDelay * 2;

            animator.SetBool("isRunning", false);
            animator.SetBool("isJumping", false);
            animator.SetBool("isSitting", false);

            // player_UI
            playerUI.animator.SetBool("isHitting", true);
            playerUI.animator.SetBool("isRunning", false);
            playerUI.animator.SetBool("isSitting", false);
        }
        spriteRenderer.color = originalColor;
        isInvincible = false;
        isStunned = false;

        // player_UI
        playerUI.animator.SetBool("isHitting", false); // Đặt lại trạng thái hoạt hình
    }

    private void ShootCurrentBullet()
    {
        if (bulletSpawnPoint == null) return;

        float direction = transform.localScale.x; // Hướng mặt người chơi (1: phải, -1: trái)

        if (bulletLevel == 1)
        {
            if (bulletLv1Prefab == null) return;

            GameObject bullet = Instantiate(bulletLv1Prefab, bulletSpawnPoint.position, Quaternion.identity);// Tạo một viên đạn cấp 1 tại vị trí spawn point
            float angle = angleBulletLlv1 * Mathf.Deg2Rad; // Góc bắn 30 độ
            Vector2 shootDir = new Vector2(Mathf.Cos(angle) * direction, -Mathf.Sin(angle)); // Bắn theo góc 30 độ

            bullet.GetComponent<FireBulletLv1>().Launch(shootDir); // Gọi phương thức Launch trên viên đạn với hướng và tốc độ
            audioManager.PlayFireBulletLv1();// Phát âm thanh bắn đạn cấp 1
        }
        else if (bulletLevel >= 2)
        {
            if (bulletLv2Prefab == null) return;

            GameObject bullet = Instantiate(bulletLv2Prefab, bulletSpawnPoint.position, Quaternion.identity);// Tạo một viên đạn cấp 2 tại vị trí spawn point
            Vector2 shootDir = new Vector2(direction, 0); // Bắn ngang

            bullet.GetComponent<FireBulletLv2>().Launch(shootDir);// Gọi phương thức Launch trên viên đạn với hướng và tốc độ
            audioManager.PlayFireBulletLv2();// Phát âm thanh bắn đạn cấp 2
        }
        // Bạn có thể mở rộng tiếp bulletLevel == 3, 4... tại đây
    }

    private void SlashCurrent()
    {
        if (slashLevel == 1 && slashComboLv1 != null)
        {
            Vector2 slashDirection = new Vector2(transform.localScale.x, 0);
            slashComboLv1.Launch(slashDirection);
            audioManager.PlayComboSlashingLv1(); // Phát âm thanh chém combo cấp 1
        }
        else if (slashLevel >= 2 && slashComboLv2 != null)
        {
            Vector2 slashDirection = new Vector2(transform.localScale.x, 0);
            slashComboLv2.Launch(slashDirection);
            audioManager.PlayComboSlashingLv2(); // Phát âm thanh chém combo cấp 1

        }
        // Bạn có thể mở rộng tiếp slashLevel == 3, 4... tại đây
    }

    private void HandleSkill()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (skill == 1 && Time.time - lastFireTime >= fireCooldown)
            {
                ShootCurrentBullet();
                lastFireTime = Time.time;
            }
            else if (skill == 2)
            {
                SlashCurrent();
            }
        }
    }

}
