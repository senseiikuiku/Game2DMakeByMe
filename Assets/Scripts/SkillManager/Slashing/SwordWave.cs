using UnityEngine;

public class SwordWave : MonoBehaviour
{

    [SerializeField] private float destroyDelay = 1f; // Thời gian trước khi phá hủy hiệu ứng sóng kiếm
    void Start()
    {
        Destroy(gameObject, destroyDelay); // Tự động phá hủy đối tượng sau một khoảng thời gian
    }

    void Update()
    {

    }
}
