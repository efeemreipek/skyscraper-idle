using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioMixer audioMixer;
    [SerializeField] private Slider musicSlider;
    [SerializeField] private Slider SFXSlider;
    [SerializeField] private TMP_Text musicText;
    [SerializeField] private TMP_Text SFXText;


    private void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        musicText.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
        SFXText.text = Mathf.RoundToInt(SFXSlider.value * 100).ToString();
    }

    public void SetMusicVolume(float volume)
    {
        if(volume <= 0.001f)
        {
            audioMixer.SetFloat("MusicVolume", -80f);
        }
        else
        { 
            audioMixer.SetFloat("MusicVolume", Mathf.Log10(volume) * 20);
        }

        musicText.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
    }
    public void SetSFXVolume(float volume)
    {
        if(volume <= 0.001f)
        {
            audioMixer.SetFloat("SFXVolume", -80f);
        }
        else
        {
            audioMixer.SetFloat("SFXVolume", Mathf.Log10(volume) * 20);
        }

        SFXText.text = Mathf.RoundToInt(SFXSlider.value * 100).ToString();
    }
}
