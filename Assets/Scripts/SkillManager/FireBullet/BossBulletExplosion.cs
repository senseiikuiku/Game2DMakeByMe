using UnityEngine;

public class BossBulletExplosion : MonoBehaviour
{
    [SerializeField] private float destroyDelay = 0.5f; // Thời gian trước khi phá hủy hiệu ứng nổ
    void Start()
    {
        Destroy(gameObject, destroyDelay); // Tự động phá hủy đối tượng sau một khoảng thời gian
    }

    void Update()
    {

    }
}
