using System.Collections;
using UnityEngine;

public class MysteryBlock : MonoBehaviour
{
    [SerializeField] private GameObject skillFireBulletPrefab; // Prefab của kỹ năng bắn đạn
    [SerializeField] private GameObject skillSlashingPrefab; // Prefab của kỹ năng chém 
    public Transform mysteryBlockSpawnPoint; // Vị trí xuất hiện của MysteryBlock

    private bool isUsed = false; // Biến để kiểm tra xem MysteryBlock đã được sử dụng hay chưa

    public bool reset = false;   // Reset lại mysteryBlock


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra có va chạm với Player không
        if (collision.gameObject.CompareTag("Player") && !isUsed)
        {
            // Kiểm tra hướng va chạm từ bên dưới
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // Va chạm từ dưới
                {
                    SpawnRandomSkill();
                    isUsed = true;
                    StartResetTimer();
                    break;
                }
            }
        }
        else if ((collision.gameObject.CompareTag("BulletLv1") || collision.gameObject.CompareTag("BulletLv2")) && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true;
            StartResetTimer();
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if ((collision.gameObject.CompareTag("SlashingLv1") ||
             collision.gameObject.CompareTag("SlashingLv2") ||
             collision.gameObject.CompareTag("SwordWave")) && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true;
            StartResetTimer();

            if (collision.gameObject.CompareTag("SwordWave"))
            {
                Destroy(collision.gameObject); // Xóa SwordWave
            }
        }
    }
    private void SpawnRandomSkill()
    {
        int randomSkill = Random.Range(0, 2); // Chọn ngẫu nhiên giữa 0 và 1
        GameObject skillPrefab = randomSkill == 0 ? skillFireBulletPrefab : skillSlashingPrefab; // Chọn prefab dựa trên giá trị ngẫu nhiên
        // Tạo một đối tượng mới từ prefab tại vị trí của MysteryBlock
        Instantiate(skillPrefab, mysteryBlockSpawnPoint.position, Quaternion.identity);
    }

    private void StartResetTimer()
    {
        if (reset) // Chỉ reset nếu bật
        {
            StopAllCoroutines();
            StartCoroutine(ResetAfterDelay(10f)); // 10 giây
        }
    }

    private IEnumerator ResetAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        isUsed = false;
    }
}
