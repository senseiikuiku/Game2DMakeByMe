using UnityEngine;
using UnityEngine.InputSystem;
using Unity.Cinemachine;

public class PlayerCollision : MonoBehaviour
{
    private AudioManager audioManager;
    private PlayerController playerController;
    private TeleportationPortal portal;
    private Revive revive;
    private Gate gate;
    private InfoKey infoKey;

    private int keyCount = 0;


    private void Awake()
    {
        audioManager = FindAnyObjectByType<AudioManager>();
        playerController = FindAnyObjectByType<PlayerController>();
        gate = FindAnyObjectByType<Gate>();
        infoKey = FindAnyObjectByType<InfoKey>();
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
                break;

            case "TeleportationPortal":
                portal = collision.gameObject.GetComponent<TeleportationPortal>();
                if (!portal.destinationImg.activeSelf)
                {
                    portal.destinationImg.SetActive(true);
                }
                if (!portal.hasTeleported)
                {
                    if (portal != null)
                    {
                        // đặt lại vị trí người chơi đến điểm đến của cổng dịch chuyển
                        transform.position = new Vector3(portal.GetDestination().position.x, portal.GetDestination().position.y, transform.position.z);
                        portal.hasTeleported = true;
                        Invoke(nameof(portal.ResetTeleportFlag), 0.5f);
                        FixCameraZ();// sửa camera về vị trí chính xác
                    }
                }
                break;

            case "ReviveA":
                revive = collision.gameObject.GetComponent<Revive>();
                if (!revive.isReviveA)
                {
                    if (revive != null)
                    {
                        // đặt lại vị trí người chơi
                        transform.position = new Vector3(revive.GetReviveB().position.x, revive.GetReviveB().position.y, transform.position.z); // đặt lại vị trí người chơi
                        revive.isReviveA = true;
                        revive.Invoke(nameof(Revive.ResetReviveFlag), 0.5f); // Reset lại cờ để hồi sinh tiếp
                        FixCameraZ();// sửa camera về vị trí chính xác
                        playerController.TakeDamage(collision.transform.position);// gọi hàm TakeDamage để giảm mạng
                    }
                }
                break;
            case "CloseGate":
                if (gate != null)
                {
                    gate.keyCount = GameManager.Instance.scoreKey;
                    gate.keyCountText.text = gate.keyCount.ToString();
                    // Nếu số chìa khóa lớn hơn hoặc = 3 và đang mở closeGate
                    if (gate.keyCount >= 3 && gate.gateItems[0].activeSelf)
                    {
                        gate.gateItems[0].SetActive(false); // đóng gameobject closeGate
                        gate.gateItems[1].SetActive(true); // mở gameobject openGate
                    }
                    else if (gate.keyCount < 3)
                    {
                        if (infoKey != null)
                            // Cập nhật thông báo key còn thiếu
                            infoKey.ShowInfo(3 - gate.keyCount);
                    }
                }
                break;
            case "OpenGate":
                if (gate != null && gate.keyCount >= 3)
                    GameManager.Instance?.GameWin();
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
}
