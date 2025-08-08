using UnityEngine;
using System.Collections;

public class BrokenGround : TrapGroundManager
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        // Kiểm tra nếu Player đứng trên nó (chạm từ phía trên)
        if (collision.gameObject.CompareTag("Player"))
        {
            // Kiểm tra nếu player ở phía trên
            if (collision.contacts[0].normal.y == -1)
            {
                if (!isShaking)
                {
                    StartCoroutine(ShakeAndFall());
                }
            }
        }
    }

    private IEnumerator ShakeAndFall()
    {
        isShaking = true;
        float elapsed = 0f;// Biến đếm thời gian đã trôi qua

        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeMagnitude; // Tạo độ lệch ngẫu nhiên
            transform.position = originalPosition + randomOffset; // Rung lắc vị trí
            elapsed += Time.deltaTime;
            yield return null;
        }

        transform.position = originalPosition; // Reset vị trí
        DestroyOrReLive(); // Gọi hàm để hủy hoặc hồi sinh nền đất

        // isShaking = false; // không cần nếu Destroy


        // Thay dòng Destroy(gameObject) nếu muốn rớt xuống
        //Rigidbody2D rb = gameObject.AddComponent<Rigidbody2D>();
        //rb.gravityScale = 2f;

    }
}
