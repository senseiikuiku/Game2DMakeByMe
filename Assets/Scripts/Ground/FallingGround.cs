using System.Collections;
using UnityEngine;

public class FallingGround : TrapGroundManager
{


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !isShaking)
        {
            // Kiểm tra nếu player đứng trên nó (chạm từ phía trên)
            if (collision.contacts[0].normal.y == -1)
            {
                StartCoroutine(ShakeAndFall());
            }
        }
    }

    private IEnumerator ShakeAndFall()
    {
        isShaking = true;
        float timer = 0f; // Biến đếm thời gian

        while (timer < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeMagnitude; // Tạo độ lệch ngẫu nhiên
            transform.position = originalPosition + randomOffset; // Rung lắc vị trí
            timer += Time.deltaTime; // Cập nhật thời gian đã trôi qua
            yield return null; // Chờ đến khung hình tiếp theo
        }

        transform.position = originalPosition; // Reset vị trí về ban đầu

        rb.bodyType = RigidbodyType2D.Dynamic; // Đặt Rigidbody thành động để rơi xuống

        rb.gravityScale = 1.2f; // Tăng trọng lực để rơi nhanh hơn

        Invoke(nameof(DestroyOrReLive), 1f); // Gọi hàm để hủy hoặc hồi sinh nền đất sau 1 giây

    }


}
