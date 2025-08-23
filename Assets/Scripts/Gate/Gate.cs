using UnityEngine;
using TMPro;
public class Gate : MonoBehaviour
{
    [SerializeField] public GameObject[] gateItems;
    [SerializeField] public TextMeshPro keyCountText;
    public int keyCount;

    private void Awake()
    {
        gateItems[0].SetActive(true); // mở gameobject closeGate
        gateItems[1].SetActive(false); // tắt gameobject openGate
    }

}
