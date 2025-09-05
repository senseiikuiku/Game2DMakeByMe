using UnityEngine;
using UnityEngine.Rendering.Universal; // cần thêm nếu dùng GlobalLight2D

public class DarkArea : MonoBehaviour
{
    [SerializeField] private Light2D globalLight2D; // Global Light 2D trong URP
    [SerializeField] private Light2D sun; // Light 2D đại diện cho ánh sáng mặt trời
    [SerializeField] private float fadeSpeed = 2f;
    private PlayerCollision playerCollision;
    private PlayerController playerController;

    private void Awake()
    {
        playerCollision = FindAnyObjectByType<PlayerCollision>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void Update()
    {
        //if (playerCollision == null) return;

        //if (playerCollision.isInDarkArea)
        //{
        //    // Giảm ánh sáng dần khi player ở vùng tối
        //    globalLight2D.intensity = Mathf.Lerp(globalLight2D.intensity, 0.3f, fadeSpeed * Time.deltaTime);
        //    // Giảm ánh sáng mặt trời
        //    sun.intensity = Mathf.Lerp(sun.intensity, 0f, fadeSpeed * Time.deltaTime);
        //}
        //else
        //{
        //    // Tăng sáng lại khi player ra khỏi vùng tối
        //    globalLight2D.intensity = Mathf.Lerp(globalLight2D.intensity, 1f, fadeSpeed * Time.deltaTime);
        //    // Tăng sáng mặt trời
        //    sun.intensity = Mathf.Lerp(sun.intensity, 5f, fadeSpeed * Time.deltaTime);
        //}
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            globalLight2D.intensity = 0.3f;
            sun.intensity = 0f;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            globalLight2D.intensity = 1f;
            sun.intensity = 5f;
        }
    }
}
