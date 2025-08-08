using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class BossAutomatic : MonoBehaviour
{
    private int facingDirection = 1; // Biến để theo dõi hướng của boss
    [SerializeField] private float speed = 3f; // boss di chuyển với tốc độ này
    [SerializeField] private float limitDistance = 2f; // khoảng cách tối đa để boss di chuyển đến người chơi

    [SerializeField] private float jumpForce = 7f; // lực nhảy của boss
    private bool isGrounded = true; // trạng thái boss có đang chạm đất không
    [SerializeField] private Transform groundCheck;
    [SerializeField] private LayerMask groundLayer;
    private float groundCheckRadius = 0.2f;

    [SerializeField] private float attackRadius = 1f; // Nếu boss tầm trung

    private bool isDead = false; // Trạng thái chết của boss
    private bool isHurt = false; // Trạng thái bị thương của boss
    private bool isPlayerHurt = false; // Trạng thái người chơi bị thương
    private bool isAttacking = false; // Trạng thái tấn công của boss


    private PlayerController playerController;
    private GameManager gameManager; // Tham chiếu đến GameManager để xử lý điểm số
    private Rigidbody2D rb;
    private ParticleSystem runEffect; // Hiệu ứng chạy (nếu cần, có thể để trống nếu không sử dụng)
    private Animator animator;
    private AudioManager audioManager; // Tham chiếu đến AudioManager để phát âm thanh

    [SerializeField] private float maxHp = 100f; // Máu tối đa của boss
    private float currentHp; // Máu hiện tại của boss
    [SerializeField] private Image hpBar;

    private enum BossMode
    {
        NormalAttack,
        DashOrStrike,
        Shooting
    }

    [SerializeField] private BossMode currentMode = BossMode.NormalAttack;


    private bool isShootingMode = false; // Chế độ bắn đạn
    [SerializeField] private GameObject fireballPrefab; // Prefab của đạn 
    [SerializeField] private Transform fireballSpawnPoint; // Vị trí bắn đạn
    [SerializeField] private float shootInterval = 2f;// Mỗi 2 giây bắn 1 viên
    private float shootTimer = 0f; // Bộ đếm thời gian để bắn đạn

    private bool isDashingNow = false;
    [SerializeField] private float dashLimitDistance = 4f; // Khoảng cách lướt tối đa
    [SerializeField] private float dashSpeed = 5f; // Tốc độ lướt
    [SerializeField] private float dashDuration = 1f;
    [SerializeField] private GameObject dashWindEffectPrefab; // Hiệu ứng gió khi lướt
    [SerializeField] private Transform dashWindSpawnPoint; // Vị trí xuất hiện hiệu ứng gió khi lướt

    [SerializeField] private int blockAttackPlayer = 3; // Số lần chặn tấn công của người chơi trước khi đánh boss
    private float timerBlockAttack = 0f; // Bộ đếm thời gian để chặn tấn công của người chơi

    private void Awake()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        gameManager = FindAnyObjectByType<GameManager>(); // Tìm GameManager trong cảnh
        rb = GetComponent<Rigidbody2D>();
        runEffect = GetComponentInChildren<ParticleSystem>(); // Lấy hiệu ứng chạy từ con cái (nếu có)   
        animator = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>(); // Tìm AudioManager trong cảnh
    }

    private void Start()
    {
        currentHp = maxHp; // Khởi tạo máu hiện tại bằng máu tối đa
    }
    private void Update()
    {
        if (playerController == null || isDead)
            return;

        UpdateState();

        switch (currentMode)
        {
            case BossMode.NormalAttack:
                MoveToPlayerAndAttack();
                FollowPlayerJump();
                FlipBoss();
                break;

            case BossMode.DashOrStrike:
                MoveDashOrStrike();
                FollowPlayerJump();
                FlipBoss();
                break;

            case BossMode.Shooting:
                shootTimer += Time.deltaTime;
                if (shootTimer >= shootInterval)
                {
                    shootTimer = 0f;
                    StartCoroutine(CastAndShootCoroutine());
                }

                SetWalking(false);
                SetAttacking(false);
                FacePlayerOnly();
                break;
        }

        // Reset lại số lần chặn tấn công nếu đã hết
        if (blockAttackPlayer <= 0)
        {
            ResetBlockAttackPlayer(); // Reset số lần chặn tấn công sau mỗi 10 giây
        }
    }

    private void UpdateState()
    {
        float hpPercent = (currentHp / maxHp) * 100;

        if (hpPercent > 50)
        {
            currentMode = BossMode.NormalAttack;
        }
        else if (hpPercent > 30)
        {
            currentMode = BossMode.Shooting;
        }
        else
        {
            currentMode = BossMode.DashOrStrike;
        }
    }


    private void MoveDashOrStrike()
    {
        if (playerController == null || isDead || isHurt || isPlayerHurt)
            return;


        float distanceToPlayer = Vector2.Distance(transform.position, playerController.transform.position);
        Debug.Log("Distance to Player: " + distanceToPlayer);

        if (distanceToPlayer > dashLimitDistance && !isDashingNow)
        {
            // Nếu trong khoảng lướt nhưng chưa đủ gần để đánh
            StartCoroutine(DashTowardsPlayer());
        }
        else if (distanceToPlayer > limitDistance && distanceToPlayer <= dashLimitDistance
            && !isHurt && !isDead)
        {
            // Nếu xa hơn khoảng lướt thì chạy bộ
            MoveToPlayer();
        }
        else if (distanceToPlayer <= limitDistance && !isAttacking && !isPlayerHurt && !isHurt)
        {
            StartCoroutine(AttackStrikeRoutine()); // Bắt đầu tấn công cận chiến
        }
    }

    private IEnumerator DashTowardsPlayer()
    {
        SetWalking(false); // Tắt hoạt ảnh đi bộ
        isDashingNow = true;
        SetCrouching(true);
        yield return DashTowardsPlayerCoroutine(); // Đợi một chút trước khi lướt
    }

    private IEnumerator DashTowardsPlayerCoroutine()
    {
        float crouchAnimLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "BossCrouch")?.length + 0.5f ?? 2f; // Lấy độ dài hoạt ảnh ngồi, nếu không tìm thấy thì mặc định là 1 giây
        yield return new WaitForSeconds(crouchAnimLength); // Thời gian chờ trước khi lướt
        SetCrouching(false); // Tắt hoạt ảnh ngồi sau khi lướt
        audioManager.PlayDashBoss(); // Phát âm thanh lướt
        // Tạo hiệu ứng gió khi lướt
        if (dashWindEffectPrefab != null && dashWindSpawnPoint != null)
        {
            if (facingDirection == 1)
            {
                dashWindEffectPrefab.transform.localScale = new Vector3(1, 1, 1); // Lật hiệu ứng gió sang phải
            }
            else
            {
                dashWindEffectPrefab.transform.localScale = new Vector3(-1, 1, 1); // Lật hiệu ứng gió sang trái
            }
            // Tạo hiệu ứng gió khi lướt
            Instantiate(dashWindEffectPrefab, dashWindSpawnPoint.position, dashWindSpawnPoint.rotation); // Tạo hiệu ứng gió khi lướt
        }
        SetDashing(true); // Bật hoạt ảnh lướt
        Debug.Log("Boss is dashing towards the player");
        Vector3 dashDirection = (playerController.transform.position - transform.position).normalized; // Tính toán hướng lướt
        float timer = 0f; // Bộ đếm thời gian lướt
        while (timer < dashDuration)
        {
            rb.linearVelocity = new Vector2(dashDirection.x * dashSpeed, 0f);
            timer += Time.deltaTime;
            yield return null;
        }
        rb.linearVelocity = Vector2.zero; // Dừng lại sau khi lướt xong
        SetDashing(false);
        isDashingNow = false;
    }

    private void MoveToPlayer()
    {
        Vector3 direction = playerController.transform.position - transform.position;
        Vector3 targetPosition = playerController.transform.position - direction.normalized * limitDistance;

        transform.position = Vector2.MoveTowards(transform.position, targetPosition, speed * Time.deltaTime);
        SetWalking(true);
        SetAttacking(false); // Tắt hoạt ảnh tấn công khi di chuyển
    }



    private void MoveToPlayerAndAttack()
    {
        if (playerController != null && !isDead && !isHurt && !isPlayerHurt && !isAttacking)
        {
            Vector3 direction = playerController.transform.position - transform.position; // tính toán hướng từ boss đến người chơi
            float distance = direction.magnitude;// tính toán khoảng cách giữa boss và người chơi
            // kiểm tra nếu khoảng cách lớn hơn giới hạn thì boss sẽ di chuyển về phía người chơi
            if (distance > limitDistance)
            {

                MoveToPlayer();
            }
            else
            {
                StartCoroutine(AttackRoutine()); // Nếu khoảng cách nhỏ hơn giới hạn, boss sẽ tấn công
            }
        }
    }

    private IEnumerator AttackRoutine()
    {
        audioManager.PlaySlashBoss(); // Phát âm thanh tấn công
        SetWalking(false); // Tắt hoạt ảnh đi bộ
        SetAttacking(true); // Bật hoạt ảnh tấn công
        isAttacking = true; // Đánh dấu boss đang tấn công

        yield return new WaitForSeconds(1); // Thời gian tấn công
        isAttacking = false; // Đánh dấu boss không còn tấn công
    }

    private IEnumerator AttackStrikeRoutine()
    {
        audioManager.PlayStrikeBoss(); // Phát âm thanh tấn công cận chiến
        SetWalking(false); // Tắt hoạt ảnh đi bộ
        SetStriking(true); // Bật hoạt ảnh cận chiến
        isAttacking = true; // Đánh dấu boss đang tấn công
        float strikeAnimLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "BossStrike")?.length + 0.1f ?? 1f; // Lấy độ dài hoạt ảnh cận chiến, nếu không tìm thấy thì mặc định là 1 giây
        yield return new WaitForSeconds(strikeAnimLength);
        isAttacking = false; // Đánh dấu boss không còn tấn công
        SetStriking(false);
    }

    private void FollowPlayerJump()
    {
        if (playerController != null && IsPlayerAbove() && IsGrounded())
        {
            audioManager.PlayJumpBoss(); // Phát âm thanh nhảy
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce); // boss nhảy lên
            SetJumpAttacking(true);
            SetWalking(false); // Tắt hoạt ảnh đi bộ khi nhảy tấn công
            SetAttacking(false); // Tắt hoạt ảnh tấn công khi nhảy tấn công

            StartCoroutine(DisableJumpAttackEffectAfterDelay(0.5f));// Tắt hoạt ảnh nhảy tấn công sau 0.5 giây
        }
    }

    // Kiểm tra xem boss có đang ở trên mặt đất không
    private bool IsGrounded()
    {
        isGrounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundLayer);
        return isGrounded;
    }

    private bool IsPlayerAbove()
    {
        if (playerController == null) return false;

        Vector3 playerPos = playerController.transform.position;
        Vector3 bossPos = transform.position;

        float horizontalRange = 1.5f; // phạm vi ngang boss sẽ phát hiện người chơi phía trên

        bool isAbove = playerPos.y > bossPos.y + 0.5f; // player cao hơn boss một chút
        bool isWithinHorizontalRange = Mathf.Abs(playerPos.x - bossPos.x) <= horizontalRange;

        return isAbove && isWithinHorizontalRange;
    }


    private void FlipBoss()
    {
        // Lật mặt boss dựa trên vị trí của người chơi
        if (playerController != null)
        {
            transform.localScale = new Vector3(playerController.transform.position.x < transform.position.x ? -1 : 1, 1, 1);
        }
        // Nếu người chơi ở bên trái boss, lật mặt sang trái, ngược lại lật mặt sang phải
        if (playerController != null && playerController.transform.position.x < transform.position.x)
        {
            facingDirection = -1; // Lật mặt sang trái
        }
        else
        {
            facingDirection = 1; // Lật mặt sang phải
        }
    }

    private void FacePlayerOnly()
    {
        if (playerController != null)
        {
            float dirX = playerController.transform.position.x - transform.position.x;
            transform.localScale = new Vector3(dirX < 0 ? -1 : 1, 1, 1);
        }
    }


    private void SetWalking(bool isWalking)
    {
        animator.SetBool("isWalking", isWalking);
    }

    private void SetAttacking(bool isAttacking)
    {
        if (isAttacking)
        {
            animator.SetTrigger("isAttacking");
        }
        else
        {
            animator.ResetTrigger("isAttacking");
        }
    }

    private void SetJumpAttacking(bool isJumpAttacking)
    {
        animator.SetBool("isJumpAttacking", isJumpAttacking); // Chạy hoạt ảnh nhảy tấn công
        audioManager.PlaySlashBoss(); // Phát âm thanh tấn công khi nhảy
    }

    private void SetCastting(bool isCasting)
    {
        animator.SetBool("isCasting", isCasting); // Tắt hoạt ảnh bắn đạn
    }

    private void SetCrouching(bool isCrouching)
    {
        animator.SetBool("isCrouching", isCrouching); // Tắt hoạt ảnh ngồi
    }

    private void SetStriking(bool isStriking)
    {
        animator.SetBool("isStriking", isStriking); // Chạy hoạt ảnh cận chiến
    }

    private void SetDashing(bool isDashing)
    {
        animator.SetBool("isDashing", isDashing); // Tắt hoạt ảnh lướt
    }

    private void SetDefending(bool isDefending)
    {
        animator.SetBool("isDefending", isDefending); // Chạy hoạt ảnh phòng thủ
    }

    private void SetHurt(bool isHurting)
    {
        animator.SetBool("isHurting", isHurting); // Chạy hoạt ảnh bị thương
        audioManager.PlayHurtBoss(); // Phát âm thanh bị thương

        if (isHurting)
        {
            isHurt = true; // Đánh dấu boss đang bị thương

        }
        else
        {
            isHurt = false; // Đánh dấu boss không còn bị thương
        }
    }

    private IEnumerator DisableDefendEffectAfterDelay()
    {
        float defendAnimLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "BossDef")?.length + 0.1f ?? 1f; // Lấy độ dài hoạt ảnh ngồi, nếu không tìm thấy thì mặc định là 1 giây
        yield return new WaitForSeconds(defendAnimLength);
        SetDefending(false); // Tắt hoạt ảnh ngồi sau delay

        rb.constraints = RigidbodyConstraints2D.FreezeRotation; // Đặt lại ràng buộc của Rigidbody2D
    }



    private IEnumerator DisableJumpAttackEffectAfterDelay(float delay)
    {

        yield return new WaitForSeconds(delay);
        SetJumpAttacking(false);// Tắt hoạt ảnh nhảy tấn công sau delay
        Debug.Log("Disable Jump Attack Effect After Delay"); // Ghi log để kiểm tra
    }

    private IEnumerator DisableHurtEffectAfterDelay(float delay)
    {
        float hurtAnimLength = animator.runtimeAnimatorController.animationClips
           .FirstOrDefault(clip => clip.name == "BossHurt")?.length + 0.1f ?? 1f;
        yield return new WaitForSeconds(hurtAnimLength);
        SetHurt(false); // Tắt hoạt ảnh bị thương sau delay

        rb.constraints = RigidbodyConstraints2D.FreezeRotation;

    }

    private IEnumerator DisablePlayerHurtEffectAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isPlayerHurt = false;
    }

    private void Die()
    {
        animator.SetBool("isDead", true); // Chạy hoạt ảnh chết
        runEffect.Play(); // Chạy hiệu ứng chạy nếu có 

        // Lấy độ dài animation chết
        float deathAnimLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "BossDie")?.length + 0.8f ?? 2f; // Nếu không tìm thấy, mặc định là 2 giây

        Destroy(gameObject, deathAnimLength);
    }

    private void TakeDamage(float damage)
    {
        currentHp -= damage; // Giảm máu boss

        if (currentHp <= 0)
            SetHurt(false); // Tắt hoạt ảnh bị thương nếu máu dưới 0

        UpdateHpBar(); // Cập nhật thanh máu

        if (!isShootingMode && currentHp <= maxHp / 2)
        {
            isShootingMode = true; // Chuyển sang chế độ bắn đạn khi máu dưới 50%
        }

        if (currentHp <= 0)
        {
            isDead = true; // Đánh dấu boss đã chết
            audioManager.PlayDieBoss(); // Phát âm thanh chết
            Die(); // Gọi hàm chết nếu máu <= 0
        }
    }

    private void UpdateHpBar()
    {
        if (hpBar != null)
        {
            hpBar.fillAmount = currentHp / maxHp; // Cập nhật thanh máu
        }
    }


    private void ResetBlockAttackPlayer()
    {
        timerBlockAttack += Time.deltaTime; // Tăng bộ đếm thời gian
        if (timerBlockAttack >= 10)
        {
            blockAttackPlayer = 3; // Đặt lại số lần chặn tấn công về 3
            timerBlockAttack = 0f; // Đặt lại bộ đếm thời gian
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        HandlePlayerAttack(collision.gameObject); // Gọi hàm xử lý va chạm với người chơi
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        HandlePlayerAttack(collision.gameObject); // Gọi hàm xử lý va chạm với người chơi
    }

    private void HandlePlayerAttack(GameObject hitObejct)
    {
        if (playerController == null)
            return;

        float damage = 0f;
        bool shouldDestroy = false; ;

        switch (hitObejct.tag)
        {
            case "BulletLv1":
                damage = 3f;
                shouldDestroy = true; // Xóa viên đạn cấp 1 sau khi va chạm
                break;
            case "BulletLv2":
                damage = 8f;
                shouldDestroy = true; // Xóa viên đạn cấp 2 sau khi va chạm
                break;
            case "SlashingLv1":
                damage = 5f;
                break;
            case "SlashingLv2":
                damage = 10f;
                break;
            case "SwordWave":
                damage = 13f;
                shouldDestroy = true; // Xóa sóng kiếm sau khi va chạm
                break;
            default:
                return; // Không xử lý nếu không phải là đạn hoặc sóng kiếm
        }

        if (blockAttackPlayer > 0)
        {
            blockAttackPlayer--; // Giảm số lần chặn tấn công
            SetWalking(false); // Tắt hoạt ảnh đi bộ
            rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;
            audioManager.PlayDefBoss(); // Phát âm thanh chặn tấn công
            SetDefending(true); // Bật hoạt ảnh phòng thủ
            StartCoroutine(DisableDefendEffectAfterDelay()); // Tắt hoạt ảnh phòng thủ sau một khoảng thời gian
            return; // Không xử lý va chạm nếu còn khả năng chặn
        }

        // Boss chịu sát thương
        SetHurt(true); // Bật hoạt ảnh bị thương
        rb.constraints = RigidbodyConstraints2D.FreezePositionX | RigidbodyConstraints2D.FreezeRotation;

        TakeDamage(damage); // Giảm máu boss
        StartCoroutine(DisableHurtEffectAfterDelay(0.5f)); // Tắt hoạt ảnh bị thương sau 0.5 giây
        if (shouldDestroy)
        {
            Destroy(hitObejct); // Xóa đối tượng va chạm nếu cần
        }
    }

    private void ShootFireBallTowardsPlayer()
    {
        if (fireballPrefab != null && fireballSpawnPoint != null && playerController != null)
        {
            GameObject fireball = Instantiate(fireballPrefab, fireballSpawnPoint.position, Quaternion.identity);
            audioManager.PlayFireBulletBoss(); // Phát âm thanh bắn đạn
            Vector2 shootDirection = (playerController.transform.position - fireballSpawnPoint.position).normalized;

            fireball.GetComponent<BossFireBullet>().Launch(shootDirection); // Đảm bảo đạn bay đúng hướng
        }
    }

    private IEnumerator CastAndShootCoroutine()
    {
        SetCastting(true); // Bật hoạt ảnh bắn đạn

        float castAnimLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "BossCast")?.length + 0.1f ?? 2f;

        yield return new WaitForSeconds(castAnimLength); // Chờ hoạt ảnh chạy

        ShootFireBallTowardsPlayer(); // Bắn đạn sau khi anim xong
        SetCastting(false); // Tắt hoạt ảnh
    }



    public void AnimationEvent()
    {
        // Kiểm tra va chạm tại thời điểm animation nếu cần 
        Vector2 attackCenter = transform.position + new Vector3(transform.localScale.x > 0 ? 1f : -1f, 0, 0); // hướng đánh

        Collider2D[] hits = Physics2D.OverlapCircleAll(attackCenter, attackRadius); // lấy tất cả các collider trong bán kính tấn công

        foreach (Collider2D hit in hits)
        {
            if (hit.CompareTag("Player") && !playerController.isInvincible)
            {

                AnimatorStateInfo stateInfo = animator.GetCurrentAnimatorStateInfo(0); // lấy thông tin trạng thái hiện tại của animator
                if (stateInfo.IsName("BossAttack") || stateInfo.IsName("BossJumpAttack") || stateInfo.IsName("BossStrike"))
                {
                    //playerController.TakeDamage(transform.position);
                    isPlayerHurt = true; // Đánh dấu boss đang bị thương
                    StartCoroutine(DisablePlayerHurtEffectAfterDelay(3f)); // Tắt trạng thái người chơi bị thương sau 0.5 giây

                }
            }
        }
    }

    // Dùng để quan sát bán kính khi boss tấn công
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Vector2 attackCenter = transform.position + new Vector3(transform.localScale.x > 0 ? 1f : -1f, 0, 0);
        Gizmos.DrawWireSphere(attackCenter, 1.1f); // same as attackRadius
    }



}
