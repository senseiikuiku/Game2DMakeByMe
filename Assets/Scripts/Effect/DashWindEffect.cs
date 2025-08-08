using System.Collections;
using UnityEngine;

public class DashWindEffect : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    void Start()
    {
        StartCoroutine(DestroyAfterAnimation()); // Bắt đầu coroutine để tự động phá hủy sau khi animation kết thúc
    }

    void Update()
    {

    }

    private IEnumerator DestroyAfterAnimation()
    {
        // Chờ 1 frame để animator có thời gian cập nhật state
        yield return null;

        // Lấy thời gian animation hiện tại
        float delay = animator.GetCurrentAnimatorStateInfo(0).length;

        yield return new WaitForSeconds(delay);
        Destroy(gameObject);
    }
}
