using System;
using System.Collections.Generic;
using System.Linq;
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
    private float musicVolume;
    private float SFXVolume;

    [Header("Resolution")]
    [SerializeField] private TMP_Dropdown resolutionDropdown;
    private List<string> resolutionOptions = new List<string>
    {
        "1024 x 576",
        "1280 x 720",
        "1366 x 768",
        "1600 x 900",
        "1920 x 1080",
        "2560 x 1440",
        "3840 x 2160",
    };
    private int resolutionWidth, resolutionHeight;

    [Header("Fullscreen")]
    [SerializeField] private TMP_Dropdown fullScreenDropdown;
    private List<string> fullScreenOptions = new List<string>
    {
        "Exclusive Fullscreen",
        "Borderless Window",
        "Windowed"
    };
    private FullScreenMode fullScreenMode;

    [Header("Quality")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private int qualityIndex;

    private void Start()
    {
        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        musicText.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
        SFXText.text = Mathf.RoundToInt(SFXSlider.value * 100).ToString();

        SetResolutionCurrent();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        SetFullScreenCurrent();
        fullScreenDropdown.onValueChanged.AddListener(SetFullScreen);

        SetQualityCurrent();
        qualityDropdown.onValueChanged.AddListener(SetQuality);
    }

    public void ApplyButton()
    {
        if(musicVolume <= 0.001f) audioMixer.SetFloat("MusicVolume", -80f);
        else audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        if(SFXVolume <= 0.001f) audioMixer.SetFloat("SFXVolume", -80f);
        else audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume) * 20);

        Screen.SetResolution(resolutionWidth, resolutionHeight, fullScreenMode);

        QualitySettings.SetQualityLevel(qualityIndex);
    }

    public void SetMusicVolume(float volume)
    {
        musicVolume = volume;
        musicText.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
    }
    public void SetSFXVolume(float volume)
    {
        SFXVolume = volume;
        SFXText.text = Mathf.RoundToInt(SFXSlider.value * 100).ToString();
    }
    public void SetResolution(int index)
    {
        string[] parts = resolutionDropdown.options[index].text.Split('x');
        resolutionWidth = int.Parse(parts[0].Trim());
        resolutionHeight = int.Parse(parts[1].Trim());
    }
    private void SetResolutionCurrent()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

        int currentWidth = Screen.width;
        int currentHeight = Screen.height;
        bool foundResolution = false;

        for(int i = 0; i < resolutionOptions.Count; i++)
        {
            string[] parts = resolutionOptions[i].Split('x');
            int w = int.Parse(parts[0].Trim());
            int h = int.Parse(parts[1].Trim());

            if(w == currentWidth && h == currentHeight)
            {
                resolutionDropdown.value = i;

                resolutionWidth = w;
                resolutionHeight = h;

                foundResolution = true;
                break;
            }
        }

        if(!foundResolution)
        {
            resolutionOptions.Add($"{currentWidth} x {currentHeight}");
            resolutionDropdown.ClearOptions();
            resolutionDropdown.AddOptions(resolutionOptions);
            resolutionDropdown.value = resolutionOptions.Count - 1;

            resolutionWidth = currentWidth;
            resolutionHeight = currentHeight;
        }
    }
    public void SetFullScreen(int index)
    {
        switch(index)
        {
            case 0:
                fullScreenMode = FullScreenMode.ExclusiveFullScreen;
                break;
            case 1:
                fullScreenMode = FullScreenMode.FullScreenWindow;
                break;
            case 2:
                fullScreenMode = FullScreenMode.Windowed;
                break;
        }
    }
    private void SetFullScreenCurrent()
    {
        fullScreenDropdown.ClearOptions();
        fullScreenDropdown.AddOptions(fullScreenOptions);

        FullScreenMode current = Screen.fullScreenMode;

        int index = 1;
        switch(current)
        {
            case FullScreenMode.ExclusiveFullScreen: index = 0; fullScreenMode = FullScreenMode.ExclusiveFullScreen; break;
            case FullScreenMode.FullScreenWindow:    index = 1; fullScreenMode = FullScreenMode.FullScreenWindow; break;
            case FullScreenMode.Windowed:            index = 2; fullScreenMode = FullScreenMode.Windowed; break;
        }

        fullScreenDropdown.value = index;
    }
    public void SetQuality(int index)
    {
        qualityIndex = index;
    }
    private void SetQualityCurrent()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(QualitySettings.names.ToList());

        int currentLevel = QualitySettings.GetQualityLevel();
        qualityDropdown.value = currentLevel;
    }
}
