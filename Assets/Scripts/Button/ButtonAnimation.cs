using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAnimation : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
{
    //Button btn;
    private Vector3 hoverScale = new Vector3(1.2f, 1.2f, 1f);
    private Vector3 downScale = new Vector3(0.9f, 0.9f, 1f);


    public void OnPointerEnter(PointerEventData eventData)
    {
        // Khi hover
        LeanTween.scale(gameObject, hoverScale, 0.1f).setIgnoreTimeScale(true);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        // Khi thoát hover
        LeanTween.scale(gameObject, Vector3.one, 0.1f).setIgnoreTimeScale(true);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        // Khi nhấn xuống
        LeanTween.scale(gameObject, downScale, 0.05f).setIgnoreTimeScale(true);
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        // Khi thả ra
        LeanTween.scale(gameObject, hoverScale, 0.1f).setIgnoreTimeScale(true);
    }
}
