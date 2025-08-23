using UnityEngine;

public abstract class TrapGroundManager : MonoBehaviour
{
    [SerializeField] protected float shakeDuration = 1f; // Thời gian rung lắc
    [SerializeField] protected float shakeMagnitude = 0.05f; // Độ mạnh của rung lắc

    protected Vector3 originalPosition; // Vị trí ban đầu của nền đất
    protected bool isShaking = false; // Biến kiểm tra xem nền đất có đang rung lắc không

    [SerializeField] protected bool reLive = false; // Biến kiểm tra có hồi sinh nền đất sau khi rơi không


    protected Rigidbody2D rb; // Rigidbody2D của nền đất

    protected void Awake()
    {
        rb = GetComponent<Rigidbody2D>(); // Lấy Rigidbody2D của nền đất
    }

    protected virtual void Start()
    {
        originalPosition = transform.position; // Lưu vị trí ban đầu của nền đất
    }

    protected void RespawnGround()
    {
        gameObject.SetActive(true);
        GetComponent<Renderer>().enabled = true; // hiện lại
        rb.bodyType = RigidbodyType2D.Kinematic; // Đặt Rigidbody thành tĩnh để không bị ảnh hưởng bởi trọng lực
        rb.gravityScale = 0f; // Đặt trọng lực về 0 để không rơi xuống
        rb.linearVelocity = Vector2.zero; // Đặt trọng lực về 0 để không rơi xuống
        transform.position = originalPosition; // Đặt lại vị trí về ban đầu
        isShaking = false; // Đặt trạng thái rung lắc về false
    }

    protected void DestroyOrReLive()
    {
        if (reLive)
        {
            GetComponent<Renderer>().enabled = false; // ẩn

            Invoke(nameof(RespawnGround), 3f); // Gọi hàm hồi sinh nền đất sau 3 giây
        }
        else
        {
            Destroy(gameObject, 0.5f); // Hủy đối tượng sau 2 giây (có thể điều chỉnh thời gian này)
        }
    }

}
