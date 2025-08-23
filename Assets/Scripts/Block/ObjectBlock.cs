using System.Collections;
using UnityEngine;

public class ObjectBlock : MonoBehaviour
{
    [SerializeField] private GameObject[] food; // Mảng chứa các loại thực phẩm
    public Transform objectBlockSpawnPoint; // Vị trí xuất hiện của ObjectBlock

    private bool isUsed = false; // Biến để kiểm tra xem ObjectBlock đã được sử dụng hay chưa

    public bool reset = false;   // Reset lại mysteryBlock


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isUsed)
        {
            // Kiểm tra hướng va chạm từ bên dưới
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // Va chạm từ dưới
                {
                    SpawnRandomFood();
                    isUsed = true;
                    StartResetTimer();
                    break;
                }
            }
        }
        else if ((collision.gameObject.CompareTag("BulletLv1") || collision.gameObject.CompareTag("BulletLv2")) && !isUsed)
        {
            SpawnRandomFood();
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
            SpawnRandomFood();
            isUsed = true;
            StartResetTimer();

            if (collision.gameObject.CompareTag("SwordWave"))
            {
                Destroy(collision.gameObject); // Xóa SwordWave
            }
        }
    }

    private void SpawnRandomFood()
    {
        if (food.Length == 0) return; // Kiểm tra mảng thực phẩm có rỗng không
        // Chọn ngẫu nhiên một loại thực phẩm từ mảng
        int randomIndex = Random.Range(0, food.Length);
        GameObject selectedFood = food[randomIndex];
        // Tạo thực phẩm tại vị trí xuất hiện
        Instantiate(selectedFood, objectBlockSpawnPoint.position, Quaternion.identity);
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
