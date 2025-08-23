using UnityEngine;
using System.Collections;

public class MysteryBlockCoin : MonoBehaviour
{
    [SerializeField] private GameObject coinPrefab; // Prefab của vàng
    public Transform mysteryBlockCoinSpawnPoint;    // Vị trí xuất hiện của vàng

    private bool isUsed = false; // Kiểm tra block đã được dùng chưa
    public bool reset = false;   // Reset lại mysteryBlock

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isUsed)
        {
            foreach (ContactPoint2D contact in collision.contacts)
            {
                if (contact.normal.y > 0.5f) // Va chạm từ dưới
                {
                    SpawnCoin();
                    isUsed = true;
                    StartResetTimer();
                    break;
                }
            }
        }
        else if ((collision.gameObject.CompareTag("BulletLv1") || collision.gameObject.CompareTag("BulletLv2")) && !isUsed)
        {
            SpawnCoin();
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
            SpawnCoin();
            isUsed = true;
            StartResetTimer();

            if (collision.gameObject.CompareTag("SwordWave"))
            {
                Destroy(collision.gameObject); // Xóa SwordWave
            }
        }
    }

    private void SpawnCoin()
    {
        Instantiate(coinPrefab, mysteryBlockCoinSpawnPoint.position, Quaternion.identity);
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
