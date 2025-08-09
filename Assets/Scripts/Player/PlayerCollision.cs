using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerCollision : MonoBehaviour
{
    private AudioManager audioManager;
    private PlayerController playerController;
    private int keyCount = 0;

    private bool hasTeleported = false;
    private bool isReviveA = false;

    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        playerController = FindAnyObjectByType<PlayerController>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        switch (collision.tag)
        {
            case "Coin":
                Destroy(collision.gameObject);
                audioManager?.PlayCoinSound();
                GameManager.Instance?.AddScore(1);
                break;

            case "Trap":
                if (!playerController.isInvincible) // Kiểm tra người chơi có đang bất tử không
                {
                    playerController.TakeDamage(collision.transform.position);
                }
                break;

            case "Key":
                Destroy(collision.gameObject);
                keyCount++;
                audioManager?.PlayKeySound();
                GameManager.Instance?.AddScoreKey(1);
                if (keyCount >= 3)
                    GameManager.Instance?.GameWin();
                break;

            case "TeleportationPortal":
                if (!hasTeleported)
                {
                    TeleportationPortal portal = collision.GetComponent<TeleportationPortal>();
                    if (portal != null)
                    {
                        // đặt lại vị trí người chơi đến điểm đến của cổng dịch chuyển
                        transform.position = new Vector3(portal.GetDestination().position.x, portal.GetDestination().position.y, transform.position.z);
                        hasTeleported = true;
                        Invoke(nameof(ResetTeleportFlag), 0.5f);
                        FixCameraZ();// sửa camera về vị trí chính xác
                    }
                }
                break;

            case "ReviveA":
                if (!isReviveA)
                {
                    Revive revive = collision.GetComponent<Revive>();
                    if (revive != null)
                    {
                        // đặt lại vị trí người chơi
                        transform.position = new Vector3(revive.GetReviveB().position.x, revive.GetReviveB().position.y, transform.position.z); // đặt lại vị trí người chơi
                        isReviveA = true;
                        Invoke(nameof(ResetReviveFlag), 0.5f);
                        FixCameraZ();// sửa camera về vị trí chính xác
                        playerController.TakeDamage(collision.transform.position);// gọi hàm TakeDamage để giảm mạng
                    }
                }
                break;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {

        switch (collision.gameObject.tag)
        {
            case "FireBallEffect":
                if (!playerController.isInvincible) // Kiểm tra người chơi có đang bất tử không
                {
                    playerController.TakeDamage(collision.transform.position);
                }
                break;
        }
    }


    public void ReduceSkillLevel()
    {
        if (playerController.bulletLevel >= 2)
        {
            playerController.bulletLevel = 1;
            playerController.skill = 1;
        }
        else if (playerController.bulletLevel == 1)
        {
            playerController.bulletLevel = 0;
            playerController.skill = 0;
        }
        else if (playerController.slashLevel >= 2)
        {
            playerController.slashLevel = 1;
            playerController.skill = 2;
        }
        else if (playerController.slashLevel == 1)
        {
            playerController.slashLevel = 0;
            playerController.skill = 0;
        }
    }

    private void FixCameraZ()
    {
        // Main Camera
        Camera mainCam = Camera.main;
        if (mainCam != null)
        {
            Vector3 camPos = mainCam.transform.position;
            camPos.z = -10f;
            mainCam.transform.position = camPos;
        }

        // Cinemachine camera
        CinemachineCamera vcam = FindAnyObjectByType<CinemachineCamera>();
        if (vcam != null && vcam.Follow != null)
        {
            Vector3 camPos = vcam.transform.position;
            camPos.z = -10f;
            vcam.transform.position = camPos;
        }
    }

    // đặt lại cờ dịch chuyển
    private void ResetTeleportFlag() => hasTeleported = false;

    // đặt lại cờ hồi sinh A
    private void ResetReviveFlag() => isReviveA = false;
}
