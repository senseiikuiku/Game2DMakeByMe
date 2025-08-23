using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] private Transform reviveB;
    public bool isReviveA = false;

    public Transform GetReviveB()
    {
        return reviveB; // Tr? v? v? trí h?i sinh
    }

    // đặt lại cờ hồi sinh A
    public void ResetReviveFlag() => isReviveA = false;
}
