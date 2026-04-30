using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.Collections.Generic;
using System.IO;

public class GameSettings : MonoBehaviour
{
    [Header("Audio")]
    public Slider musicVolumeSlider;
    public AudioSource musicSource;

    public Slider sfxVolumeSlider;
    public AudioSource[] sfxSources;

    [Header("Screen")]
    public Toggle fullscreenToggle;
    public TMP_Dropdown resolutionDropdown;
    public TMP_Dropdown qualityDropdown;

    [Header("Camera")]
    public Slider fovSlider;
    public Camera playerCamera;

    [Header("Default Settings")]
    public float defaultMusicVolume = 1f;
    public float defaultSFXVolume = 1f;
    public bool defaultFullscreen = true;
    public int defaultQualityIndex = 2;
    public float defaultFOV = 75f;

    private Resolution[] resolutions;
    private string savePath;
    private bool loadingSettings = false;

    private void Start()
    {
        savePath = Application.persistentDataPath + "/settings.json";

        SetupMusicVolume();
        SetupSFXVolume();
        SetupFOV();
        SetupFullscreen();
        SetupResolutions();
        SetupQuality();

        if (File.Exists(savePath))
        {
            LoadSettings();
        }
        else
        {
            ResetSettingsToDefault();
        }
    }

    private void SetupMusicVolume()
    {
        if (musicVolumeSlider == null || musicSource == null)
        {
            Debug.LogWarning("Music volume slider or music source is not assigned.");
            return;
        }

        musicVolumeSlider.minValue = 0f;
        musicVolumeSlider.maxValue = 1f;
        musicVolumeSlider.value = musicSource.volume;

        musicVolumeSlider.onValueChanged.RemoveAllListeners();
        musicVolumeSlider.onValueChanged.AddListener(SetMusicVolume);
    }

    private void SetupSFXVolume()
    {
        if (sfxVolumeSlider == null)
        {
            Debug.LogWarning("SFX volume slider is not assigned.");
            return;
        }

        sfxVolumeSlider.minValue = 0f;
        sfxVolumeSlider.maxValue = 1f;

        float startVolume = defaultSFXVolume;

        if (sfxSources != null && sfxSources.Length > 0 && sfxSources[0] != null)
        {
            startVolume = sfxSources[0].volume;
        }

        sfxVolumeSlider.value = startVolume;

        sfxVolumeSlider.onValueChanged.RemoveAllListeners();
        sfxVolumeSlider.onValueChanged.AddListener(SetSFXVolume);
    }

    private void SetupFOV()
    {
        if (fovSlider == null || playerCamera == null)
        {
            Debug.LogWarning("FOV slider or player camera is not assigned.");
            return;
        }

        fovSlider.minValue = 60f;
        fovSlider.maxValue = 110f;
        fovSlider.value = playerCamera.fieldOfView;

        fovSlider.onValueChanged.RemoveAllListeners();
        fovSlider.onValueChanged.AddListener(SetFOV);
    }

    private void SetupFullscreen()
    {
        if (fullscreenToggle == null)
        {
            Debug.LogWarning("Fullscreen toggle is not assigned.");
            return;
        }

        fullscreenToggle.isOn = Screen.fullScreen;

        fullscreenToggle.onValueChanged.RemoveAllListeners();
        fullscreenToggle.onValueChanged.AddListener(SetFullscreen);
    }

    private void SetupResolutions()
    {
        if (resolutionDropdown == null)
        {
            Debug.LogWarning("Resolution dropdown is not assigned.");
            return;
        }

        resolutions = Screen.resolutions;

        resolutionDropdown.ClearOptions();

        List<string> options = new List<string>();
        int currentResolutionIndex = 0;

        for (int i = 0; i < resolutions.Length; i++)
        {
            string option = resolutions[i].width + " x " + resolutions[i].height;
            options.Add(option);

            if (resolutions[i].width == Screen.currentResolution.width &&
                resolutions[i].height == Screen.currentResolution.height)
            {
                currentResolutionIndex = i;
            }
        }

        resolutionDropdown.AddOptions(options);
        resolutionDropdown.value = currentResolutionIndex;
        resolutionDropdown.RefreshShownValue();

        resolutionDropdown.onValueChanged.RemoveAllListeners();
        resolutionDropdown.onValueChanged.AddListener(SetResolution);

        Debug.Log("Resolution dropdown options loaded: " + options.Count);
    }

    private void SetupQuality()
    {
        if (qualityDropdown == null)
        {
            Debug.LogWarning("Quality dropdown is not assigned.");
            return;
        }

        qualityDropdown.ClearOptions();

        List<string> qualityOptions = new List<string>(QualitySettings.names);

        qualityDropdown.AddOptions(qualityOptions);

        int safeQualityIndex = Mathf.Clamp(QualitySettings.GetQualityLevel(), 0, QualitySettings.names.Length - 1);
        qualityDropdown.value = safeQualityIndex;
        qualityDropdown.RefreshShownValue();

        qualityDropdown.onValueChanged.RemoveAllListeners();
        qualityDropdown.onValueChanged.AddListener(SetQuality);

        Debug.Log("Quality dropdown options loaded: " + qualityOptions.Count);
    }

    public void ResetSettingsToDefault()
    {
        loadingSettings = true;

        float musicVolume = Mathf.Clamp01(defaultMusicVolume);
        float sfxVolume = Mathf.Clamp01(defaultSFXVolume);
        int qualityIndex = Mathf.Clamp(defaultQualityIndex, 0, QualitySettings.names.Length - 1);
        float fov = Mathf.Clamp(defaultFOV, 60f, 110f);

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = musicVolume;

        ApplyMusicVolume(musicVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = sfxVolume;

        ApplySFXVolume(sfxVolume);

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = defaultFullscreen;

        ApplyFullscreen(defaultFullscreen);

        if (qualityDropdown != null)
        {
            qualityDropdown.value = qualityIndex;
            qualityDropdown.RefreshShownValue();
        }

        ApplyQuality(qualityIndex);

        if (resolutionDropdown != null && resolutionDropdown.options.Count > 0)
        {
            int defaultResolutionIndex = resolutionDropdown.options.Count - 1;
            resolutionDropdown.value = defaultResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            ApplyResolution(defaultResolutionIndex);
        }

        if (fovSlider != null)
            fovSlider.value = fov;

        ApplyFOV(fov);

        loadingSettings = false;

        SaveSettings();

        Debug.Log("Settings reset to default.");
    }

    public void SetFullscreen(bool isFullscreen)
    {
        ApplyFullscreen(isFullscreen);

        if (!loadingSettings)
            SaveSettings();
    }

    public void SetResolution(int resolutionIndex)
    {
        ApplyResolution(resolutionIndex);

        if (!loadingSettings)
            SaveSettings();
    }

    public void SetQuality(int qualityIndex)
    {
        ApplyQuality(qualityIndex);

        if (!loadingSettings)
            SaveSettings();
    }

    public void SetMusicVolume(float volume)
    {
        ApplyMusicVolume(volume);

        if (!loadingSettings)
            SaveSettings();
    }

    public void SetSFXVolume(float volume)
    {
        ApplySFXVolume(volume);

        if (!loadingSettings)
            SaveSettings();
    }

    public void SetFOV(float fov)
    {
        ApplyFOV(fov);

        if (!loadingSettings)
            SaveSettings();
    }

    private void ApplyFullscreen(bool isFullscreen)
    {
        Screen.fullScreen = isFullscreen;
    }

    private void ApplyResolution(int resolutionIndex)
    {
        if (resolutions == null || resolutions.Length == 0)
            return;

        resolutionIndex = Mathf.Clamp(resolutionIndex, 0, resolutions.Length - 1);

        Resolution resolution = resolutions[resolutionIndex];

        Screen.SetResolution(
            resolution.width,
            resolution.height,
            Screen.fullScreen
        );
    }

    private void ApplyQuality(int qualityIndex)
    {
        qualityIndex = Mathf.Clamp(qualityIndex, 0, QualitySettings.names.Length - 1);
        QualitySettings.SetQualityLevel(qualityIndex);
    }

    private void ApplyMusicVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (musicSource != null)
            musicSource.volume = volume;
    }

    private void ApplySFXVolume(float volume)
    {
        volume = Mathf.Clamp01(volume);

        if (sfxSources == null)
            return;

        foreach (AudioSource source in sfxSources)
        {
            if (source != null)
                source.volume = volume;
        }
    }

    private void ApplyFOV(float fov)
    {
        if (playerCamera != null)
            playerCamera.fieldOfView = Mathf.Clamp(fov, 60f, 110f);
    }

    public void SaveSettings()
    {
        if (string.IsNullOrEmpty(savePath))
            savePath = Application.persistentDataPath + "/settings.json";

        SettingsData data = new SettingsData();

        data.musicVolume = musicVolumeSlider != null ? musicVolumeSlider.value : defaultMusicVolume;
        data.sfxVolume = sfxVolumeSlider != null ? sfxVolumeSlider.value : defaultSFXVolume;
        data.fullscreen = fullscreenToggle != null ? fullscreenToggle.isOn : defaultFullscreen;
        data.resolutionIndex = resolutionDropdown != null ? resolutionDropdown.value : 0;
        data.qualityIndex = qualityDropdown != null ? qualityDropdown.value : defaultQualityIndex;
        data.fov = fovSlider != null ? fovSlider.value : defaultFOV;

        string json = JsonUtility.ToJson(data, true);
        File.WriteAllText(savePath, json);

        Debug.Log("Settings saved to: " + savePath);
    }

    public void LoadSettings()
    {
        if (string.IsNullOrEmpty(savePath))
            savePath = Application.persistentDataPath + "/settings.json";

        if (!File.Exists(savePath))
        {
            ResetSettingsToDefault();
            return;
        }

        loadingSettings = true;

        string json = File.ReadAllText(savePath);
        SettingsData data = JsonUtility.FromJson<SettingsData>(json);

        if (musicVolumeSlider != null)
            musicVolumeSlider.value = data.musicVolume;

        ApplyMusicVolume(data.musicVolume);

        if (sfxVolumeSlider != null)
            sfxVolumeSlider.value = data.sfxVolume;

        ApplySFXVolume(data.sfxVolume);

        if (fullscreenToggle != null)
            fullscreenToggle.isOn = data.fullscreen;

        ApplyFullscreen(data.fullscreen);

        if (resolutionDropdown != null)
        {
            int safeResolutionIndex = Mathf.Clamp(data.resolutionIndex, 0, resolutionDropdown.options.Count - 1);
            resolutionDropdown.value = safeResolutionIndex;
            resolutionDropdown.RefreshShownValue();
            ApplyResolution(safeResolutionIndex);
        }

        if (qualityDropdown != null)
        {
            int safeQualityIndex = Mathf.Clamp(data.qualityIndex, 0, QualitySettings.names.Length - 1);
            qualityDropdown.value = safeQualityIndex;
            qualityDropdown.RefreshShownValue();
            ApplyQuality(safeQualityIndex);
        }

        if (fovSlider != null)
            fovSlider.value = data.fov;

        ApplyFOV(data.fov);

        loadingSettings = false;

        Debug.Log("Settings loaded from: " + savePath);
    }
}

[System.Serializable]
public class SettingsData
{
    public float musicVolume = 1f;
    public float sfxVolume = 1f;
    public bool fullscreen = true;
    public int resolutionIndex = 0;
    public int qualityIndex = 2;
    public float fov = 75f;
}