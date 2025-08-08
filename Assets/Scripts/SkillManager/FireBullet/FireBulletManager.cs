using UnityEngine;

public abstract class FireBulletManager : MonoBehaviour
{
    [SerializeField] protected float speed = 10f; // Tốc độ của viên đạn
    [SerializeField] protected float lifetime = 2f; // Thời gian sống của viên đạn

    protected Rigidbody2D rb;
    protected AudioManager audioManager; // Tham chiếu đến AudioManager để phát âm thanh
    protected float timer;
    protected virtual void Awake()
    {
        rb = GetComponent<Rigidbody2D>();
        audioManager = FindAnyObjectByType<AudioManager>(); // Tìm AudioManager trong scene
    }
    protected virtual void Start()
    {

    }

    protected virtual void Update()
    {
        TimerLifetime();
    }


    public void Launch(Vector2 direction)
    {
        // Thiết lập vận tốc ban đầu
        rb.linearVelocity = direction.normalized * speed;

        if (direction.x > 0)
        {
            transform.localScale = new Vector3(1f, 1f, 1f); // Quay mặt sang phải
        }
        else if (direction.x < 0)
        {
            transform.localScale = new Vector3(-1f, 1f, 1f); // Quay mặt sang trái
        }
    }

    protected void TimerLifetime()
    {
        timer += Time.deltaTime;
        if (timer >= lifetime)
        {
            Destroy(gameObject); // Tự hủy sau thời gian sống
        }
    }
}
