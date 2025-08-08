using UnityEngine;

public class MysteryBlock : MonoBehaviour
{
    [SerializeField] private GameObject skillFireBulletPrefab; // Prefab của kỹ năng bắn đạn
    [SerializeField] private GameObject skillSlashingPrefab; // Prefab của kỹ năng chém 
    public Transform mysteryBlockSpawnPoint; // Vị trí xuất hiện của MysteryBlock

    private bool isUsed = false; // Biến để kiểm tra xem MysteryBlock đã được sử dụng hay chưa



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
                    break;
                }
            }
        }
        else if (collision.gameObject.CompareTag("BulletLv1") && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true; // Đánh dấu là đã sử dụng
        }
        else if (collision.gameObject.CompareTag("BulletLv2") && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true; // Đánh dấu là đã sử dụng
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlashingLv1") && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true; // Đánh dấu là đã sử dụng
        }
        else if (collision.gameObject.CompareTag("SlashingLv2") && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true; // Đánh dấu là đã sử dụng
        }
        else if (collision.gameObject.CompareTag("SwordWave") && !isUsed)
        {
            SpawnRandomSkill();
            isUsed = true; // Đánh dấu là đã sử dụng
            Destroy(collision.gameObject); // Xóa đối tượng SwordWave sau khi va chạm
        }
    }
    private void SpawnRandomSkill()
    {
        int randomSkill = Random.Range(0, 2); // Chọn ngẫu nhiên giữa 0 và 1
        GameObject skillPrefab = randomSkill == 0 ? skillFireBulletPrefab : skillSlashingPrefab; // Chọn prefab dựa trên giá trị ngẫu nhiên
        // Tạo một đối tượng mới từ prefab tại vị trí của MysteryBlock
        Instantiate(skillPrefab, mysteryBlockSpawnPoint.position, Quaternion.identity);
    }
}
