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

    [SerializeField] float brightnessMax;
    [SerializeField] float brightnessMin;
    [SerializeField] float xSensMax;
    [SerializeField] float xSensMin;
    [SerializeField] float ySensMax;
    [SerializeField] float ySensMin;
    [SerializeField] float fovMax;
    [SerializeField] float fovMin;

    public GameObject settingsCanvas;
    Settings settingsScript;
    Resolution[] resolutions;
    List<string> options = new List<string>();
    List<Resolution> filteredResolutions;
    double currentRefreshRate;
    int currentResolutionIndex;

    [SerializeField] TMP_Text brightnessTxt;
    [SerializeField] TMP_Text resolutionTxt;
    [SerializeField] TMP_Text fovTxt;
    [SerializeField] TMP_Text sensXTxt;
    [SerializeField] TMP_Text sensYTxt;

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
                case "SettingsCanvas":
                    settingsCanvas = goArray[i];
                    break;
            }
        }
        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;
    }

    // Start is called before the first frame update
    void Start()
    {
        ResolutionList();
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

        brightnessTxt.text = brightness.ToString();
        resolutionTxt.text = options[PlayerPrefs.GetInt("Resolution")];
        sensXTxt.text = PlayerPrefs.GetFloat("XSens").ToString();
        sensYTxt.text = PlayerPrefs.GetFloat("YSens").ToString();
        fovTxt.text = PlayerPrefs.GetFloat("FOV").ToString();
    }

    void ResolutionList()
    {
        for (int i = 0; i < resolutions.Length; i++)
        {
            if (resolutions[i].refreshRateRatio.value == currentRefreshRate)
            {
                filteredResolutions.Add(resolutions[i]);
            }
        }

        for (int i = 0; i < filteredResolutions.Count; i++)
        {
            string resolutionOption = filteredResolutions[i].width + "x" + filteredResolutions[i].height + " " + filteredResolutions[i].refreshRateRatio.value + " Hz";
            options.Add(resolutionOption);
            if (filteredResolutions[i].width == Screen.width && filteredResolutions[i].height == Screen.height)
            {
                currentResolutionIndex = i;
            }
        }

        if (!PlayerPrefs.HasKey("Resolution"))
        {
            PlayerPrefs.SetInt("Resolution", currentResolutionIndex);
            PlayerPrefs.Save();
        }
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
    }

    public void ResolutionSet(int resolutionIndex)
    {
        Resolution resolution = filteredResolutions[resolutionIndex];
        Screen.SetResolution(resolution.width, resolution.height, Screen.fullScreen);
        PlayerPrefs.SetInt("Resolution", resolutionIndex);
        PlayerPrefs.Save();
    }
    public void ResolutionUp()
    {
        if (currentResolutionIndex > filteredResolutions.Count)
        {
            currentResolutionIndex = filteredResolutions.Count;
        }
        else
        {
            currentResolutionIndex++;
        }
        ResolutionSet(currentResolutionIndex);
    }
    public void ResolutionDown()
    {
        if (currentResolutionIndex < 0)
        {
            currentResolutionIndex = 0;
        }
        else
        {
            currentResolutionIndex--;
        }
        ResolutionSet(currentResolutionIndex);
    }

    public void SettingsClose()
    {
        settingsCanvas.SetActive(false);
    }

    public void FullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", (fullscreenToggle.isOn ? 1 : 0));
        PlayerPrefs.Save();
    }

    public void BrightnessUp()
    {
        if (PlayerPrefs.GetFloat("Brightness") >= brightnessMax)
        {
            PlayerPrefs.SetFloat("Brightness", brightnessMax);
        }
        else
        {
            PlayerPrefs.SetFloat("Brightness", MathF.Round(PlayerPrefs.GetFloat("Brightness") + 0.1f, 1));
        }
        settingsScript.liftGammagGain.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Brightness"));
        PlayerPrefs.Save();
    }
    public void BrightnessDown()
    {
        if (PlayerPrefs.GetFloat("Brightness") <= brightnessMin)
        {
            PlayerPrefs.SetFloat("Brightness", brightnessMin);
        }
        else
        {
            PlayerPrefs.SetFloat("Brightness", MathF.Round(PlayerPrefs.GetFloat("Brightness") - 0.1f, 1));
        }
        settingsScript.liftGammagGain.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Brightness"));
        PlayerPrefs.Save();
    }

    public void SensXUp()
    {
        if (PlayerPrefs.GetFloat("XSens") >= xSensMax)
        {
            PlayerPrefs.SetFloat("XSens", xSensMax);
        }
        else
        {
            PlayerPrefs.SetFloat("XSens", PlayerPrefs.GetFloat("XSens") + 1);
        }
        PlayerPrefs.Save();
    }
    public void SensXDown()
    {
        if (PlayerPrefs.GetFloat("XSens") <= xSensMin)
        {
            PlayerPrefs.SetFloat("XSens", xSensMin);
        }
        else
        {
            PlayerPrefs.SetFloat("XSens", PlayerPrefs.GetFloat("XSens") - 1);
        }
        PlayerPrefs.Save();
    }

    public void SensYUp()
    {
        if (PlayerPrefs.GetFloat("YSens") >= ySensMax)
        {
            PlayerPrefs.SetFloat("YSens", ySensMax);
        }
        else
        {
            PlayerPrefs.SetFloat("YSens", PlayerPrefs.GetFloat("YSens") + 1);
        }
        PlayerPrefs.Save();
    }
    public void SensYDown()
    {
        if (PlayerPrefs.GetFloat("YSens") <= ySensMin)
        {
            PlayerPrefs.SetFloat("YSens", ySensMin);
        }
        else
        {
            PlayerPrefs.SetFloat("YSens", PlayerPrefs.GetFloat("YSens") - 1);
        }
        PlayerPrefs.Save();
    }

    public void FOVUp()
    {
        if (PlayerPrefs.GetFloat("FOV") >= fovMax)
        {
            PlayerPrefs.SetFloat("FOV", fovMax);
        }
        else
        {
            PlayerPrefs.SetFloat("FOV", PlayerPrefs.GetFloat("FOV") + 1);
        }
        PlayerPrefs.Save();
    }
    public void FOVDown()
    {
        if (PlayerPrefs.GetFloat("FOV") <= fovMin)
        {
            PlayerPrefs.SetFloat("FOV", fovMin);
        }
        else
        {
            PlayerPrefs.SetFloat("FOV", PlayerPrefs.GetFloat("FOV") - 1);
        }
        PlayerPrefs.Save();
    }
}
