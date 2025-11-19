using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class VolumeController : MonoBehaviour
{
    public AudioMixer mixer;   
    public Slider slider;     

    private void Start()
    {
        float savedValue = PlayerPrefs.GetFloat("MusicVolume", 1f);
        slider.value = savedValue;
        SetMusicVolume(savedValue);
    }

    public void SetMusicVolume(float value)
    {
        float volumeInDb = Mathf.Lerp(-80f, 0f, value);
        mixer.SetFloat("MusicVolume", volumeInDb);

        PlayerPrefs.SetFloat("MusicVolume", value);
    }
}