using UnityEngine;
using System.Collections;

public class MovingPlatform : MonoBehaviour
{
    [SerializeField] private Transform pointA;
    [SerializeField] private Transform pointB;
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển của platform
    private Vector3 target;
    private Transform player;
    void Start()
    {
        target = pointA.position;
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);
        if (Vector3.Distance(transform.position, target) < 0.1f)
        {
            if (target == pointA.position)
            {
                target = pointB.position; // Chuyển sang điểm B
            }
            else
            {
                target = pointA.position; //Chuyên sang điểm A
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            float playerY = collision.transform.position.y; // Vị trí Y của người chơi khi họ va chạm với platform
            float platformY = transform.position.y;// Vị trí Y của platform
            Debug.Log("Platform Y: " + platformY);

            // Nếu player đứng cao hơn platform một chút
            if (playerY > platformY + 0.3f) // 0.3 có thể điều chỉnh
            {
                collision.transform.SetParent(transform);// Đặt người chơi làm con của platform
            }
        }
    }


    public void OnCollisionExit2D(Collision2D collision)
    {
        if (!gameObject.activeInHierarchy) return; // Kiểm tra xem platform có đang hoạt động không

        // Khi người chơi rời khỏi platform, xóa parent của người chơi
        if (collision.gameObject.CompareTag("Player"))
        {
            StartCoroutine(RemoveParentDelayed(collision.transform)); // Gọi hàm để xóa parent sau 1 frame
        }
    }

    private IEnumerator RemoveParentDelayed(Transform player)
    {
        yield return null; // đợi 1 frame
        if (player != null)
        {
            player.SetParent(null);
        }
    }

    void OnDisable() // Hàm này sẽ được gọi khi platform bị vô hiệu hóa
    {
        StopAllCoroutines(); // Dừng tất cả các coroutine khi platform bị vô hiệu hóa
    }

}
