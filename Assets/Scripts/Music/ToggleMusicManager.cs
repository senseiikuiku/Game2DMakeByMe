using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public abstract class ToggleMusicManager : MonoBehaviour
{
    [SerializeField] protected Image image; // Nút để bật/tắt nhạc
    [SerializeField] protected Sprite[] musicIcon; // Mảng chứa các icon nhạc
    [SerializeField] protected AudioSource audioSorce;
    [SerializeField] protected Slider slider; // Âm thanh nhạc nền
    protected bool isMusicOn = true;
    public string musicOnKey;
    protected virtual void Start()
    {
        isMusicOn = PlayerPrefs.GetFloat(musicOnKey, 1f) > 0f; // Lấy giá trị từ PlayerPrefs
        UpdateMusicState(isMusicOn);
    }

    protected virtual void Update()
    {

    }

    protected void UpdateMusicState(bool state)
    {
        // Kiểm tra null hoặc thiếu phần tử
        if (musicIcon == null || musicIcon.Length < 2 || image == null || slider == null || audioSorce == null)
        {
            Debug.LogError("Thiếu tham chiếu trong Inspector (musicIcon, image, slider, audioSorce)");
            return;
        }

        // Chỉ gán icon theo trạng thái: 0 = bật, 1 = tắt
        if (musicIcon.Length >= 2 && image != null)
        {
            image.sprite = state ? musicIcon[0] : musicIcon[1];
        }

        // Cập nhật âm lượng của AudioSource
        if (image.sprite == musicIcon[1])
        {
            audioSorce.volume = 0f; // Tắt âm thanh
            slider.value = 0f; // Đặt giá trị âm lượng của slider về 0
        }
        else
        {
            audioSorce.volume = PlayerPrefs.GetFloat(musicOnKey, 1f); // Lấy giá trị âm lượng từ PlayerPrefs
            slider.value = audioSorce.volume; // Đặt giá trị âm lượng của slider về giá trị hiện tại
        }

        // Bật/tắt âm thanh nhạc nền
        if (audioSorce != null)
            audioSorce.mute = !state;
    }

    public void OnButtonClick()
    {
        isMusicOn = !isMusicOn;
        UpdateMusicState(isMusicOn);
        PlayerPrefs.SetFloat(musicOnKey, isMusicOn ? audioSorce.volume : 0f); // Lưu trạng thái vào PlayerPrefs
        PlayerPrefs.Save(); // Lưu thay đổi
    }
}
