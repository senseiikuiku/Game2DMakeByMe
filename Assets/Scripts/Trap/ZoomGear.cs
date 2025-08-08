using UnityEngine;

public class ZoomGear : MonoBehaviour
{
    [SerializeField] private Vector3 minScale = new Vector3(1f, 1f, 1f);
    [SerializeField] private Vector3 maxScale = new Vector3(1.5f, 1.5f, 1f);
    [SerializeField] private float zoomDuration = 1f;// Thời gian để zoom in/out

    private float timer = 0f;// Thời gian đã trôi qua
    private bool zoomingIn = true;// Biến để xác định hướng zoom

    void Update()
    {
        timer += Time.deltaTime;
        float t = timer / zoomDuration; // Tính toán tỷ lệ thời gian đã trôi qua so với tổng thời gian zoom
        t = Mathf.Clamp01(t);// Giới hạn giá trị t trong khoảng [0, 1]

        if (zoomingIn)
        {
            transform.localScale = Vector3.Lerp(minScale, maxScale, t);
        }
        else
        {
            transform.localScale = Vector3.Lerp(maxScale, minScale, t);
        }

        if (t >= 1f)
        {
            zoomingIn = !zoomingIn;  // Đảo chiều
            timer = 0f;              // Reset thời gian cho chu kỳ mới
        }
    }
}
