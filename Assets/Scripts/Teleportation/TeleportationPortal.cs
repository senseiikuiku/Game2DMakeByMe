using UnityEngine;

public class TeleportationPortal : MonoBehaviour
{
    [SerializeField] public GameObject destinationImg;
    [SerializeField] private Transform destination; // Điểm đến của portal
    public bool hasTeleported = false;
    public bool teleportAgain = false;

    private void Awake()
    {
        destinationImg.SetActive(false);
    }

    public Transform GetDestination()
    {
        return destination; // Trả về điểm đến của portal
    }

    // đặt lại cờ dịch chuyển
    public void ResetTeleportFlag()
    {
        if (teleportAgain)
            hasTeleported = false; // Dịch chuyển lại
        else
            hasTeleported = true; // Dịch chuyển được 1 lần
    }
}
