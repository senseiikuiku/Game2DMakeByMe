using UnityEngine;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour
{
    Button btn;
    Vector3 upScale = new Vector3(1.2f, 1.2f, 1f);
    private void Awake()
    {
        btn = gameObject.GetComponent<Button>();
        btn.onClick.AddListener(Anim); // Đăng ký sự kiện khi nút được nhấn
    }
    public void Anim()
    {
        LeanTween.scale(gameObject, upScale, 0.1f).setIgnoreTimeScale(true);// Tăng kích thước nút
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setDelay(0.1f).setIgnoreTimeScale(true);// Trở về kích thước ban đầu sau một khoảng thời gian
    }
}
