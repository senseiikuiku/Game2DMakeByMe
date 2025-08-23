using UnityEngine;
using TMPro;
using System.Collections;

public class InfoKey : MonoBehaviour
{
    [SerializeField] private GameObject infoKey;
    [SerializeField] private TextMeshPro keyCountText;

    private void Start()
    {
        infoKey.SetActive(false);
    }

    public void ShowInfo(int remainingKeys)
    {
        // Nếu đang hiển thị thì bỏ qua
        if (infoKey.activeSelf) return;

        keyCountText.text = remainingKeys.ToString();
        infoKey.SetActive(true);
        StartCoroutine(HideAfterSeconds(3f));
    }

    private IEnumerator HideAfterSeconds(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        infoKey.SetActive(false);
    }
}
