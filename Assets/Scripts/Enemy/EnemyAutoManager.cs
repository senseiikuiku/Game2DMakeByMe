using System.Collections;
using System.Linq;
using UnityEngine;

public abstract class EnemyAutoManager : MonoBehaviour
{
    [SerializeField] protected float enemyMoveSpeed = 1f;
    protected bool isDead = false; // Để kiểm tra xem kẻ địch đã chết chưa
    protected bool isAttack = false;// Kiểm slime đã đánh chưa

    [SerializeField] protected float distanceToPlayer = 5f; // Khoảng cách tối thiểu để bắt đầu di chuyển về phía người chơi



    private Animator animator; // Thành phần hoạt hình cho hoạt ảnh (nếu cần)
    protected PlayerController playerController;
    protected AudioManager audioManager; // Tham chiếu đến AudioManager để quản lý âm thanh
    protected CircleCollider2D circleCollider2D; // Tham chiếu đến CircleCollider2D để điều chỉnh bán kính tấn công


    protected virtual void Start()
    {
        playerController = FindAnyObjectByType<PlayerController>();
        animator = GetComponent<Animator>();
        audioManager = FindAnyObjectByType<AudioManager>();
    }

    protected virtual void Update()
    {
        MoveToPlayer();
    }

    protected void MoveToPlayer()
    {
        if (playerController != null)
        {
            float distance = Vector2.Distance(transform.position, playerController.transform.position);

            if (distance <= distanceToPlayer && !isAttack) // Chỉ di chuyển khi khoảng cách <= 5 đơn vị và slime chưa đánh
            {
                // Di chuyển kẻ địch về phía người chơi
                transform.position = Vector2.MoveTowards(transform.position, playerController.transform.position, enemyMoveSpeed * Time.deltaTime);
                animator.SetBool("isRunning", true); // Bật hoạt ảnh chạy
                FlipEnemy();
            }
            else
            {
                animator.SetBool("isRunning", false); // Tắt hoạt ảnh chạy
            }
        }
        else
        {
            animator.SetBool("isRunning", false);
        }
    }


    protected void FlipEnemy()
    {
        if (playerController != null)
        {
            // Lật kẻ địch theo hướng người chơi
            transform.localScale = new Vector3(
               playerController.transform.position.x < transform.position.x ? -1 : 1,
               transform.localScale.y,
               transform.localScale.z
            );

        }
    }

    // Hàm này sẽ được gọi khi kẻ thù bị người chơi tấn công
    protected void HandleDeath(string enemySlimeDie)
    {
        isDead = true;
        animator.SetBool(enemySlimeDie, true);
        float deadAnimLength = animator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "Slime_dead")?.length + 0.1f ?? 1f; // Lấy độ dài hoạt ảnh chết, nếu không tìm thấy thì mặc định là 1 giây
        enemyMoveSpeed = 0;

        if (playerController != null)
        {
            playerController.rb.linearVelocity = new Vector2(playerController.rb.linearVelocity.x, 10f); // Đẩy người chơi lên khi kẻ thù chết
        }

        // Disable collider để player xuyên qua
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy");


        audioManager.PlayEnemyDieSound();

        Destroy(gameObject, deadAnimLength);
    }

    // Hàm này sẽ được gọi khi kẻ thù va chạm với người chơi
    protected void HandlePlayerCollision(ContactPoint2D contact, string enemySlimeDie, string nameEnemySlime)
    {
        bool stomped = contact.normal.y < -0.5f; // Kiểm tra va chạm từ trên xuống
        bool hitPlayerSide = contact.normal.y >= -0.5f; // Kiểm tra va chạm từ bên cạnh hoặc dưới lên

        if (stomped)
        {
            HandleDeath(enemySlimeDie);
        }
        else if (hitPlayerSide)
        {
            if (playerController != null && !playerController.isInvincible)
            {
                if (nameEnemySlime == "Slime_1")
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isAttacking", true);
                    isAttack = true;
                    playerController.TakeDamage(transform.position);
                    float attackAnimLength = animator.runtimeAnimatorController.animationClips
                        .FirstOrDefault(clip => clip.name == "Slime_attack")?.length + 0.1f ?? 1f; // Lấy độ dài hoạt ảnh tấn công, nếu không tìm thấy thì mặc định là 1 giây
                    Debug.Log("Attack animation length: " + attackAnimLength);
                    Destroy(gameObject, attackAnimLength); // Xóa kẻ thù sau khi tấn công người chơi
                }
                else
                {
                    animator.SetBool("isRunning", false);
                    animator.SetBool("isAttacking", true);
                    isAttack = true;
                    playerController.TakeDamage(transform.position);
                    float attackAnimLength = animator.runtimeAnimatorController.animationClips
                        .FirstOrDefault(clip => clip.name == "Slime_attack")?.length + 0.1f ?? 1f; // Lấy độ dài hoạt ảnh tấn công, nếu không tìm thấy thì mặc định là 1 giây
                    StartCoroutine(SetAttackingFalse(attackAnimLength)); // Tắt hoạt ảnh tấn công sau khi kết thúc
                    StartCoroutine(CheckSlimeAttack()); // Đặt lại isAttack sau 2 giây
                }
            }
        }
    }

    // Hàm này kiểm tra trạng thái tấn công của slime sau 2 giây
    protected IEnumerator CheckSlimeAttack()
    {
        yield return new WaitForSeconds(2f); // Thời gian chờ 2s
        isAttack = false;
    }

    // Hàm này đặt isAttacking về false sau khi hoạt ảnh tấn công kết thúc
    protected IEnumerator SetAttackingFalse(float attackAnimLength)
    {
        yield return new WaitForSeconds(attackAnimLength); // Chờ đến khi hoạt ảnh tấn công kết thúc

        animator.SetBool("isAttacking", false);
    }

    // Vẽ bán kính tấn công trong Scene view để dễ dàng điều chỉnh
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, circleCollider2D != null ? circleCollider2D.radius : 1f);
    }


    // Hàm này sẽ được gọi khi kẻ thù bị giết bởi kỹ năng của người chơi
    protected void EnemyDieBySkill(Vector2 skillDirection, float directionX, float directionY, string enemySlimeDie)
    {
        isDead = true;
        enemyMoveSpeed = 0;

        // Văng lên trên nhẹ
        Rigidbody2D rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của kẻ thù
        if (rb != null)
        {
            rb.linearVelocity = new Vector2(skillDirection.x * directionX, directionY); // văng lên và sang ngang
            rb.gravityScale = 1.5f; // Tăng trọng lực để rơi nhanh hơn
        }

        animator.SetBool(enemySlimeDie, true);
        gameObject.layer = LayerMask.NameToLayer("DeadEnemy"); // Disable collider để player xuyên qua
        audioManager.PlayEnemyDieSound(); // Phát âm thanh khi kẻ thù chết
        Destroy(gameObject, 0.5f); // Xóa kẻ thù sau 0.5 giây
    }

    // Hàm này sẽ được gọi khi kẻ thù bị bắn bởi đạn
    protected void EnemySlimeDieByBullet(Vector2 bulletDirection)
    {
        if (isDead) { return; }

        EnemyDieBySkill(bulletDirection, 1.5f, 1.5f, "isDead"); // Gọi hàm xử lý khi kẻ thù bị bắn
    }

    // Hàm này sẽ được gọi khi kẻ thù bị chém bởi slash
    protected void EnemySlimeDieBySlash(Vector2 slashDirection)
    {
        if (isDead) { return; }

        EnemyDieBySkill(slashDirection, 1.5f, 1.5f, "isDead"); // Gọi hàm xử lý khi kẻ thù bị chém
    }

    // Hàm này sẽ được gọi khi kẻ thù bị chém bởi sóng kiếm
    protected void EnemySlimeDieBySwordWave(Vector2 swordWaveDirection)
    {
        if (isDead) { return; }

        EnemyDieBySkill(swordWaveDirection, 1.5f, 1.5f, "isDead"); // Gọi hàm xử lý khi kẻ thù bị chém bởi sóng kiếm
    }

}
