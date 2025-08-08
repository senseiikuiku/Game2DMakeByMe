using UnityEngine;

public class TeleportationPortal : MonoBehaviour
{
    [SerializeField] private Transform destination; // Điểm đến của portal
    public Transform GetDestination()
    {
        return destination; // Trả về điểm đến của portal
    }
}
