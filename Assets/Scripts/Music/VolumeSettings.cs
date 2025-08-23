using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeSettings : MonoBehaviour
{
    [SerializeField] public AudioSource backGroundAudioSource;
    [SerializeField] public AudioSource effectAudioSource;
    [SerializeField] public Slider musicSlider;
    [SerializeField] public Slider effectSlider;


    private void Start()
    {
        float musicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);// mặc định là 1f
        float effectVolume = PlayerPrefs.GetFloat("EffectVolume", 1f);
        backGroundAudioSource.volume = musicVolume;
        effectAudioSource.volume = effectVolume;

        musicSlider.value = musicVolume;
        effectSlider.value = effectVolume;

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        effectSlider.onValueChanged.AddListener(SetEffectVolume);


    }

    private void SetMusicVolume(float value)
    {
        backGroundAudioSource.volume = value;
        PlayerPrefs.SetFloat("MusicVolume", value);
        PlayerPrefs.Save();
    }

    private void SetEffectVolume(float value)
    {
        effectAudioSource.volume = value;
        PlayerPrefs.SetFloat("EffectVolume", value);
        PlayerPrefs.Save();

    }



}
