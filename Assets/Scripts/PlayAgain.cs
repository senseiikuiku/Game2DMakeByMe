using Unity.VisualScripting;
using UnityEngine;

public class PlayAgain : MonoBehaviour
{
    public void OnButtonClick()
    {
        GameManager.Instance.RestartGame();
    }
}
