using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.UI;
using Unity.VisualScripting;

public class Settings : MonoBehaviour
{
    public static Settings _instance;

    [SerializeField] int smCoin;
    [SerializeField] int deathTotal;
    [SerializeField] int winTotal;
    [SerializeField] int levelsSpawned;
    [SerializeField] float bestTime;
    [SerializeField] bool isFullscreen;
    [SerializeField] float brightness;

    Volume postProcessing;
    public LiftGammaGain liftGammagGain;

    Toggle fullscreenToggle;
    Slider brightnessSlider;
    Slider xSensSlider;
    Slider ySensSlider;
    Slider fovSlider;
    TMP_Dropdown rDropdown;
    public GameObject settingsCanvas;
    Settings settingsScript;
    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    double currentRefreshRate;
    int currentResolutionIndex;

    public static Settings Instance { get { return _instance; } }

    private void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(this.gameObject);
        }
        else
        {
            _instance = this;
            DontDestroyOnLoad(this.gameObject);
        }

        postProcessing = this.gameObject.GetComponentInChildren<Volume>();
        postProcessing.profile.TryGet(out liftGammagGain);

        if (!PlayerPrefs.HasKey("smCoin"))
        {
            PlayerPrefs.SetInt("smCoin", 0);
        }
        if (!PlayerPrefs.HasKey("deathTotal"))
        {
            PlayerPrefs.SetInt("deathTotal", 0);
        }
        if (!PlayerPrefs.HasKey("winTotal"))
        {
            PlayerPrefs.SetInt("winTotal", 0);
        }
        if (!PlayerPrefs.HasKey("levelsSpawned"))
        {
            PlayerPrefs.SetInt("levelsSpawned", 0);
        }
        if (!PlayerPrefs.HasKey("BestTime"))
        {
            PlayerPrefs.SetInt("BestTime", 9999);
        }

        GameObject[] goArray = Resources.FindObjectsOfTypeAll<GameObject>();
        for (int i = 0; i < goArray.Length; i++)
        {
            switch (goArray[i].name)
            {
                case "Fullscreen Toggle":
                    fullscreenToggle = goArray[i].GetComponent<Toggle>();
                    break;
                case "Settings Values":
                    settingsScript = goArray[i].GetComponent<Settings>();
                    break;
                case "Resolution Dropdown":
                    rDropdown = goArray[i].GetComponent<TMP_Dropdown>();
                    break;
                case "Brightness Slider":
                    brightnessSlider = goArray[i].GetComponent<Slider>();
                    break;
                case "SettingsCanvas":
                    settingsCanvas = goArray[i];
                    break;
                case "XSens Slider":
                    xSensSlider = goArray[i].GetComponent<Slider>();
                    break;
                case "YSens Slider":
                    ySensSlider = goArray[i].GetComponent<Slider>();
                    break;
                case "FOV Slider":
                    fovSlider = goArray[i].GetComponent<Slider>();
                    break;
            }
        }
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        rDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResolutionDropdown();
        SetSettings();
    }

    // Update is called once per frame
    void Update()
    {
        smCoin = PlayerPrefs.GetInt("smCoin");
        deathTotal = PlayerPrefs.GetInt("deathTotal");
        winTotal = PlayerPrefs.GetInt("winTotal");
        levelsSpawned = PlayerPrefs.GetInt("levelsSpawned");
        bestTime = PlayerPrefs.GetFloat("BestTime");
        if (PlayerPrefs.GetInt("Fullscreen") != 0)
        {
            isFullscreen = true;
        }
        else
        {
            isFullscreen = false;
        }
        brightness = PlayerPrefs.GetFloat("Brightness");
    }

    void ResolutionDropdown()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        List<string> options = new List<string>();
        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        rDropdown.AddOptions(options);

        if (!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
            PlayerPrefs.Save();
            rDropdown.value = currentResolutionIndex;
        }
        else
        {
            rDropdown.value = PlayerPrefs.GetInt("Resolution");
        }

        rDropdown.RefreshShownValue();
    }

    void SetSettings()
    {
        if (PlayerPrefs.HasKey("Brightness"))
        {
            settingsScript.liftGammagGain.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Brightness"));
        }
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            if (PlayerPrefs.GetInt("Fullscreen") != 0)
            {
                Screen.fullScreen = true;
            }
            else
            {
                Screen.fullScreen = false;
            }
        }
        if (PlayerPrefs.HasKey("Resolution"))
        {
            ResolutionSet(PlayerPrefs.GetInt("Resolution"));
        }
    }

    public void SetUi()
    {
        if (PlayerPrefs.HasKey("Fullscreen"))
        {
            if (PlayerPrefs.GetInt("Fullscreen") != 0)
            {
                fullscreenToggle.isOn = true;
            }
            else
            {
                fullscreenToggle.isOn = false;
            }
        }
        if (PlayerPrefs.HasKey("Brightness"))
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
        }
        if (PlayerPrefs.HasKey("Resolution"))
        {
            rDropdown.value = PlayerPrefs.GetInt("Resolution");
            rDropdown.RefreshShownValue();
        }
        if (PlayerPrefs.HasKey("XSens"))
        {
            xSensSlider.value = PlayerPrefs.GetFloat("XSens");
        }
        if (PlayerPrefs.HasKey("YSens"))
        {
            ySensSlider.value = PlayerPrefs.GetFloat("YSens");
        }
        if (PlayerPrefs.HasKey("FOV"))
        {
            fovSlider.value = PlayerPrefs.GetFloat("FOV");
        }
    }

    public void SettingsClose()
    {
        settingsCanvas.SetActive(false);
    }

    public void FullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", (Screen.fullScreen ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void Brightness()
    {
        PlayerPrefs.SetFloat("Brightness", brightnessSlider.value);
        settingsScript.liftGammagGain.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Brightness"));
        PlayerPrefs.Save();
    }

    public void ResolutionSet(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }

    public void SensXSet(float XSens)
    {
        PlayerPrefs.SetFloat("XSens", XSens);
        PlayerPrefs.Save();
    }

    public void SensYSet(float YSens)
    {
        PlayerPrefs.SetFloat("YSens", YSens);
        PlayerPrefs.Save();
    }

    public void FOVSet(float fov)
    {
        PlayerPrefs.SetFloat("FOV", fov);
        PlayerPrefs.Save();
    }
}
