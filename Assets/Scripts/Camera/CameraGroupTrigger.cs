using Unity.Cinemachine;
using UnityEngine;

public class CameraGroupTrigger : MonoBehaviour
{
    public CinemachineTargetGroup targetGroup;
    public Transform targetToAdd;
    public float weight = 1f;
    public float radius = 0f;

    private bool hasAdded = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!hasAdded && collision.CompareTag("Player"))
        {
            targetGroup.AddMember(targetToAdd, weight, radius);
            hasAdded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (hasAdded && collision.CompareTag("Player"))
        {
            targetGroup.RemoveMember(targetToAdd);
            hasAdded = false;
        }
    }
}
