using System.Collections;
using UnityEngine;

public class BrokenWall : TrapGroundManager
{


    private int lives = 3; // Số lần va chạm trước khi phá hủy



    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("BulletLv1"))
        {
            lives--; // Giảm số lần va chạm
            if (lives <= 0)
            {
                if (!isShaking)
                {
                    StartCoroutine(ShakeAndDestroy());
                }
            }
            else
            {
                StartCoroutine(ShakeAndNotDestroy());
            }
            Destroy(collision.gameObject); // Xóa viên đạn khi va chạm với tường
        }
        else if (collision.gameObject.CompareTag("BulletLv2") || collision.gameObject.CompareTag("BulletExplosion"))
        {
            lives = 0;
            if (lives <= 0)
            {
                if (!isShaking)
                {
                    StartCoroutine(ShakeAndDestroy());
                }
            }
            Destroy(collision.gameObject); // Xóa viên đạn khi va chạm với tường
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("SlashingLv1"))
        {
            lives--; // Giảm số lần va chạm
            if (lives <= 0)
            {
                if (!isShaking)
                {
                    StartCoroutine(ShakeAndDestroy());
                }
            }
            else
            {
                StartCoroutine(ShakeAndNotDestroy());
            }
            Destroy(collision.gameObject); // Xóa đòn chém khi va chạm với tường
        }
        else if (collision.gameObject.CompareTag("SlashingLv2") || collision.gameObject.CompareTag("SwordWave"))
        {
            lives = 0;
            if (lives <= 0)
            {
                if (!isShaking)
                {
                    StartCoroutine(ShakeAndDestroy());
                }
            }
            Destroy(collision.gameObject); // Xóa đòn chém hoặc sóng kiếm khi va chạm với tường
        }
    }

    private IEnumerator ShakeAndDestroy()
    {
        isShaking = true;
        float elapsed = 0f; // Biến đếm thời gian đã trôi qua
        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeMagnitude; // Tạo độ lệch ngẫu nhiên
            transform.position = originalPosition + randomOffset; // Rung lắc vị trí
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Reset vị trí
        DestroyOrReLive(); // Gọi hàm để hủy hoặc hồi sinh nền đất
    }

    private IEnumerator ShakeAndNotDestroy()
    {
        float elapsed = 0f; // Biến đếm thời gian đã trôi qua
        while (elapsed < shakeDuration)
        {
            Vector3 randomOffset = Random.insideUnitCircle * shakeMagnitude; // Tạo độ lệch ngẫu nhiên
            transform.position = originalPosition + randomOffset; // Rung lắc vị trí
            elapsed += Time.deltaTime;
            yield return null;
        }
        transform.position = originalPosition; // Reset vị trí
    }


}
