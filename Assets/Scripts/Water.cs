using UnityEngine;
using System.Linq; // thêm cái này

public class Water : MonoBehaviour
{
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemySlime") || collision.gameObject.CompareTag("EnemyLv1") || collision.gameObject.CompareTag("EnemyLv2"))
        {
            Debug.Log("EnemySlime touched water");
            Animator enemyAnimator = collision.gameObject.GetComponent<Animator>(); // Lấy Animator của EnemySlime
            enemyAnimator.SetBool("isDead", true); // Kích hoạt trạng thái chết
            collision.gameObject.layer = LayerMask.NameToLayer("DeadEnemy");

            float deadAnimLength = enemyAnimator.runtimeAnimatorController.animationClips
            .FirstOrDefault(clip => clip.name == "Enemy_dead")?.length + 0.1f ?? 0.5f; // Lấy độ dài hoạt ảnh chết, nếu không tìm thấy thì mặc định là 0.5 giây

            Destroy(collision.gameObject, deadAnimLength); // Hủy đối tượng sau khi hoạt ảnh chết kết thúc
        }
    }
}
