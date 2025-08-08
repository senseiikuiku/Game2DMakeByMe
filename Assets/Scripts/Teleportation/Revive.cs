using UnityEngine;

public class Revive : MonoBehaviour
{
    [SerializeField] private Transform reviveB;

    public Transform GetReviveB()
    {
        return reviveB; // Tr? v? v? trí h?i sinh
    }

}
