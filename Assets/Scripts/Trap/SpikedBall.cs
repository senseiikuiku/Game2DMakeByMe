using UnityEngine;

public class SpikedBall : MonoBehaviour
{
    [SerializeField] private float rotationSpeed = 90f; // chỉnh tay trong Unity
    [SerializeField] private float swingSpeed = 2f;     // tốc độ lắc

    private GameManager gameManager;

    private void Awake()
    {
        gameManager = FindAnyObjectByType<GameManager>();
    }

    void Update()
    {
        if (gameManager.IsGameOver() || gameManager.IsGameWin()) return;

        if (rotationSpeed > 90f)
        {
            // Quay 360 độ theo chiều kim đồng hồ
            transform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        else
        {
            // Lắc trái - phải như con lắc
            float angle = Mathf.Sin(Time.time * swingSpeed) * rotationSpeed;// tính góc lắc
            transform.rotation = Quaternion.Euler(0f, 0f, angle);// gắn góc quay vào trục Z
        }
    }
}
