using UnityEngine;

public class Gear : MonoBehaviour
{
    [SerializeField] private Transform startPoint; // Điểm bắt đầu của bánh răng
    [SerializeField] private Transform endPoint; // Điểm kết thúc của bánh răng
    [SerializeField] private float speed = 2f; // Tốc độ di chuyển của bánh răng
    private Vector3 target; // Điểm đích mà bánh răng sẽ di chuyển tới


    private void Start()
    {
        target = startPoint.position; // Bắt đầu di chuyển đến điểm kết thúc
    }

    void Update()
    {
        if (GameManager.Instance.IsGameOver() || GameManager.Instance.IsGameWin())
            return;// Nếu trò chơi đã kết thúc hoặc thắng, không thực hiện di chuyển

        // Kiểm tra khoảng cách giữa vị trí hiện tại và điểm đích
        transform.position = Vector3.MoveTowards(transform.position, target, speed * Time.deltaTime);

        if (Vector3.Distance(transform.position, target) < 0.1f) // Kiểm tra xem bánh răng đã đến gần điểm đích chưa
        {
            // Nếu đã đến điểm đích, đổi hướng
            if (target == startPoint.position)
            {
                target = endPoint.position; // Chuyển sang điểm kết thúc
            }
            else
            {
                target = startPoint.position; // Chuyển sang điểm bắt đầu
            }
        }

    }



}
