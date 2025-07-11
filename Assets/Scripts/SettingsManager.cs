using System;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class SettingsManager : Singleton<SettingsManager>
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
    private int resolutionIndex;

    [Header("Fullscreen")]
    [SerializeField] private TMP_Dropdown fullScreenDropdown;
    private List<string> fullScreenOptions = new List<string>
    {
        "Exclusive Fullscreen",
        "Borderless Window",
        "Windowed"
    };
    private FullScreenMode fullScreenMode;
    private int fullScreenIndex;

    [Header("Quality")]
    [SerializeField] private TMP_Dropdown qualityDropdown;
    private int qualityIndex;

    [Header("VSync")]
    [SerializeField] private Toggle vSyncToggle;
    private bool isVSyncOn;

    private SettingsData settingsData;
    public SettingsData SettingsData => settingsData;

    private void Start()
    {
        LoadSettings();

        musicSlider.onValueChanged.AddListener(SetMusicVolume);
        SFXSlider.onValueChanged.AddListener(SetSFXVolume);

        SetResolutionCurrent();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        SetFullScreenCurrent();
        fullScreenDropdown.onValueChanged.AddListener(SetFullScreen);

        SetQualityCurrent();
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        SetVSyncCurrent();
        vSyncToggle.onValueChanged.AddListener(SetVSync);
    }

    private void LoadSettings()
    {
        settingsData = SaveManager.Instance.LoadSettings();
        ApplySettings(settingsData);
    }
    private void ApplySettings(SettingsData settingsData)
    {
        if(settingsData != null)
        {
            musicVolume = settingsData.MusicVolume;
            SFXVolume = settingsData.SFXVolume;
            resolutionIndex = settingsData.ResolutionIndex;
            fullScreenIndex = settingsData.FullScreenIndex;
            qualityIndex = settingsData.QualityIndex;
            isVSyncOn = settingsData.IsVSyncOn;
        }
        else
        {
            musicVolume = musicSlider.value;
            SFXVolume = SFXSlider.value;
        }

        musicSlider.value = musicVolume;
        SFXSlider.value = SFXVolume;

        musicText.text = Mathf.RoundToInt(musicSlider.value * 100).ToString();
        SFXText.text = Mathf.RoundToInt(SFXSlider.value * 100).ToString();

        if(musicVolume <= 0.001f) audioMixer.SetFloat("MusicVolume", -80f);
        else audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        if(SFXVolume <= 0.001f) audioMixer.SetFloat("SFXVolume", -80f);
        else audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume) * 20);
    }

    public void ApplyButton()
    {
        if(musicVolume <= 0.001f) audioMixer.SetFloat("MusicVolume", -80f);
        else audioMixer.SetFloat("MusicVolume", Mathf.Log10(musicVolume) * 20);

        if(SFXVolume <= 0.001f) audioMixer.SetFloat("SFXVolume", -80f);
        else audioMixer.SetFloat("SFXVolume", Mathf.Log10(SFXVolume) * 20);

        Screen.SetResolution(resolutionWidth, resolutionHeight, fullScreenMode);

        QualitySettings.SetQualityLevel(qualityIndex);

        QualitySettings.vSyncCount = isVSyncOn ? 1 : 0;

        settingsData = new SettingsData()
        {
            MusicVolume = musicVolume,
            SFXVolume = SFXVolume,
            ResolutionIndex = resolutionIndex,
            FullScreenIndex = fullScreenIndex,
            QualityIndex = qualityIndex,
            IsVSyncOn = isVSyncOn
        };
        SaveManager.Instance.SaveSettings();
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
        resolutionIndex = index;

        string[] parts = resolutionDropdown.options[index].text.Split('x');
        resolutionWidth = int.Parse(parts[0].Trim());
        resolutionHeight = int.Parse(parts[1].Trim());
    }
    private void SetResolutionCurrent()
    {
        resolutionDropdown.ClearOptions();
        resolutionDropdown.AddOptions(resolutionOptions);

        if(settingsData != null)
        {
            resolutionIndex = settingsData.ResolutionIndex;

            if(resolutionIndex >= 0 && resolutionIndex < resolutionOptions.Count)
            {
                string[] parts = resolutionOptions[resolutionIndex].Split('x');
                resolutionWidth = int.Parse(parts[0].Trim());
                resolutionHeight = int.Parse(parts[1].Trim());

                resolutionDropdown.value = resolutionIndex;
                return;
            }
        }

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
                resolutionIndex = i;

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
            resolutionIndex = resolutionOptions.Count - 1;

            resolutionWidth = currentWidth;
            resolutionHeight = currentHeight;
        }
    }
    public void SetFullScreen(int index)
    {
        fullScreenIndex = index;

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

        if(settingsData != null)
        {
            fullScreenIndex = settingsData.FullScreenIndex;
            fullScreenMode = fullScreenIndex switch
            {
                0 => FullScreenMode.ExclusiveFullScreen,
                1 => FullScreenMode.FullScreenWindow,
                2 => FullScreenMode.Windowed,
                _ => Screen.fullScreenMode
            };

            fullScreenDropdown.value = fullScreenIndex;
        }
        else
        {
            fullScreenMode = Screen.fullScreenMode;
            fullScreenIndex = fullScreenMode switch
            {
                FullScreenMode.ExclusiveFullScreen => 0,
                FullScreenMode.FullScreenWindow => 1,
                FullScreenMode.Windowed => 2,
                _ => 0
            };

            fullScreenDropdown.value = fullScreenIndex;
        }
    }
    public void SetQuality(int index)
    {
        qualityIndex = index;
    }
    private void SetQualityCurrent()
    {
        qualityDropdown.ClearOptions();
        qualityDropdown.AddOptions(QualitySettings.names.ToList());

        if(settingsData != null)
        {
            qualityDropdown.value = settingsData.QualityIndex;
        }
        else
        {
            int currentLevel = QualitySettings.GetQualityLevel();
        }
    }
    public void SetVSync(bool isOn)
    {
        isVSyncOn = isOn;
    }
    private void SetVSyncCurrent()
    {
        if(settingsData != null)
        {
            vSyncToggle.isOn = settingsData.IsVSyncOn;
        }
        else
        {
            vSyncToggle.isOn = QualitySettings.vSyncCount > 0;
        }
    }
    public void ResetSaveButton()
    {
        SaveManager.Instance.DeleteSaveAndReset();
    }
}
