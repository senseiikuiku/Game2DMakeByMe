using UnityEngine;

public class UpdateRoundNumber : MonoBehaviour
{
    [SerializeField] private int gateNumber;
    [SerializeField] NextRoundGate nextRoundGate;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            if (PlayerPrefs.GetInt("KeyRound", 1) <= gateNumber)
            {
                PlayerPrefs.SetInt("KeyRound", gateNumber);
                GameManager.Instance.keyRound = gateNumber;
            }

            nextRoundGate.roundNumber = gateNumber;
        }
    }
}
