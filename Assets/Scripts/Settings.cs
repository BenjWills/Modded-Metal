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
using UnityEngine.Audio;

public class Settings : MonoBehaviour
{
    public static Settings _instance;

    [Header("PlayerPrefs Stats")]
    [SerializeField] int smCoin;
    [SerializeField] int deathTotal;
    [SerializeField] int winTotal;
    [SerializeField] int levelsSpawned;
    [SerializeField] float bestTime;
    //[SerializeField] bool isFullscreen;
    [SerializeField] float brightness;

    Volume postProcessing;
    public LiftGammaGain liftGammagGain;

    Toggle fullscreenToggle;

    [Header("Brightness")]
    [SerializeField] float brightnessMax;
    [SerializeField] float brightnessMin;

    [Header("X Sensitivity")]
    [SerializeField] float xSensMax;
    [SerializeField] float xSensMin;

    [Header("Y Sensitivity")]
    [SerializeField] float ySensMax;
    [SerializeField] float ySensMin;

    [Header("Field Of Vision")]
    [SerializeField] float fovMax;
    [SerializeField] float fovMin;

    [Header("Main Volume")]
    [SerializeField] float mainVolMax;
    [SerializeField] float mainVolMin;
    [SerializeField] AudioSource[] allAudio;

    [Header("Music")]
    [SerializeField] float musicMax;
    [SerializeField] float musicMin;
    public AudioSource[] music;

    [Header("Sound")]
    [SerializeField] float soundMax;
    [SerializeField] float soundMin;
    [SerializeField] AudioSource[] sound;

    [Header("Game Objects")]
    public GameObject settingsCanvas;
    [SerializeField] AudioMixer audioMixer;

    Settings settingsScript;
    Resolution[] resolutions;
    List<string> options = new List<string>();
    List<Resolution> filteredResolutions;
    double currentRefreshRate;
    int currentResolutionIndex;

    [Header("Text")]
    [SerializeField] TMP_Text brightnessTxt;
    [SerializeField] TMP_Text resolutionTxt;
    [SerializeField] TMP_Text fovTxt;
    [SerializeField] TMP_Text sensXTxt;
    [SerializeField] TMP_Text sensYTxt;
    [SerializeField] TMP_Text mainVolTxt;
    [SerializeField] TMP_Text musicTxt;
    [SerializeField] TMP_Text soundTxt;

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
        audioMixer.SetFloat("MainVolume", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("MainVol")) * 20, 1));
        audioMixer.SetFloat("Music", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20, 1));
        audioMixer.SetFloat("Sound", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20, 1));
    }

    // Update is called once per frame
    void Update()
    {
        SliderMax();
        SliderMin();

        smCoin = PlayerPrefs.GetInt("smCoin");
        deathTotal = PlayerPrefs.GetInt("deathTotal");
        winTotal = PlayerPrefs.GetInt("winTotal");
        levelsSpawned = PlayerPrefs.GetInt("levelsSpawned");
        bestTime = PlayerPrefs.GetFloat("BestTime");
        //if (PlayerPrefs.GetInt("Fullscreen") != 0)
        //{
        //    isFullscreen = true;
        //}
        //else
        //{
        //    isFullscreen = false;
        //}
        brightness = PlayerPrefs.GetFloat("Brightness");

        brightnessTxt.text = brightness.ToString();
        resolutionTxt.text = options[PlayerPrefs.GetInt("Resolution")];
        sensXTxt.text = PlayerPrefs.GetFloat("XSens").ToString();
        sensYTxt.text = PlayerPrefs.GetFloat("YSens").ToString();
        fovTxt.text = PlayerPrefs.GetFloat("FOV").ToString();
        mainVolTxt.text = PlayerPrefs.GetFloat("MainVol").ToString();
        soundTxt.text = PlayerPrefs.GetFloat("Sound").ToString();
        musicTxt.text = PlayerPrefs.GetFloat("Music").ToString();
    }

    public void MuteMusic()
    {
        for (int i = 0; i < music.Length; i++)
        {
            music[i].mute = true;
        }
    }
    public void UnmuteMusic()
    {
        for (int i = 0; i < music.Length; i++)
        {
            music[i].mute = false;
        }
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
        if (currentResolutionIndex >= filteredResolutions.Count - 1)
        {
            currentResolutionIndex = filteredResolutions.Count - 1;
        }
        else
        {
            currentResolutionIndex++;
        }
        ResolutionSet(currentResolutionIndex);
    }
    public void ResolutionDown()
    {
        if (currentResolutionIndex <= 0)
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
    void SliderMax()
    {
        if (PlayerPrefs.GetFloat("Brightness") >= brightnessMax)
        {
            PlayerPrefs.SetFloat("Brightness", brightnessMax);
        }
        if (PlayerPrefs.GetFloat("XSens") >= xSensMax)
        {
            PlayerPrefs.SetFloat("XSens", xSensMax);
        }
        if (PlayerPrefs.GetFloat("YSens") >= ySensMax)
        {
            PlayerPrefs.SetFloat("YSens", ySensMax);
        }
        if (PlayerPrefs.GetFloat("FOV") >= fovMax)
        {
            PlayerPrefs.SetFloat("FOV", fovMax);
        }
        if (PlayerPrefs.GetFloat("MainVol") >= mainVolMax)
        {
            PlayerPrefs.SetFloat("MainVol", mainVolMax);
        }
        if (PlayerPrefs.GetFloat("Music") >= musicMax)
        {
            PlayerPrefs.SetFloat("Music", musicMax);
        }
        if (PlayerPrefs.GetFloat("Sound") >= soundMax)
        {
            PlayerPrefs.SetFloat("Sound", soundMax);
        }
    }
    void SliderMin()
    {
        if (PlayerPrefs.GetFloat("Brightness") <= brightnessMin)
        {
            PlayerPrefs.SetFloat("Brightness", brightnessMin);
        }
        if (PlayerPrefs.GetFloat("XSens") <= xSensMin)
        {
            PlayerPrefs.SetFloat("XSens", xSensMin);
        }
        if (PlayerPrefs.GetFloat("YSens") <= ySensMin)
        {
            PlayerPrefs.SetFloat("YSens", ySensMin);
        }
        if (PlayerPrefs.GetFloat("FOV") <= fovMin)
        {
            PlayerPrefs.SetFloat("FOV", fovMin);
        }
        if (PlayerPrefs.GetFloat("MainVol") <= mainVolMin)
        {
            PlayerPrefs.SetFloat("MainVol", mainVolMin);
            for (int i = 0; i < allAudio.Length; i++)
            {
                allAudio[i].mute = true;
            }
        }
        if (PlayerPrefs.GetFloat("Music") <= musicMin)
        {
            PlayerPrefs.SetFloat("Music", musicMin);
            for (int i = 0; i < music.Length; i++)
            {
                music[i].mute = true;
            }
        }
        if (PlayerPrefs.GetFloat("Sound") <= soundMin)
        {
            PlayerPrefs.SetFloat("Sound", soundMin);
            for (int i = 0; i < sound.Length; i++)
            {
                sound[i].mute = true;
            }
        }
    }

    public void BrightnessUp()
    {
        PlayerPrefs.SetFloat("Brightness", MathF.Round(PlayerPrefs.GetFloat("Brightness") + 0.1f, 1));
        settingsScript.liftGammagGain.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Brightness"));
        PlayerPrefs.Save();
    }
    public void BrightnessDown()
    {
        PlayerPrefs.SetFloat("Brightness", MathF.Round(PlayerPrefs.GetFloat("Brightness") - 0.1f, 1));
        settingsScript.liftGammagGain.gamma.value = new Vector4(1, 1, 1, PlayerPrefs.GetFloat("Brightness"));
        PlayerPrefs.Save();
    }

    public void SensXUp()
    {
        PlayerPrefs.SetFloat("XSens", PlayerPrefs.GetFloat("XSens") + 1);
        PlayerPrefs.Save();
    }
    public void SensXUp5()
    {
        PlayerPrefs.SetFloat("XSens", PlayerPrefs.GetFloat("XSens") + 5);
        PlayerPrefs.Save();
    }
    public void SensXDown()
    {
        PlayerPrefs.SetFloat("XSens", PlayerPrefs.GetFloat("XSens") - 1);
        PlayerPrefs.Save();
    }
    public void SensXDown5()
    {
        PlayerPrefs.SetFloat("XSens", PlayerPrefs.GetFloat("XSens") - 5);
        PlayerPrefs.Save();
    }

    public void SensYUp()
    {
        PlayerPrefs.SetFloat("YSens", PlayerPrefs.GetFloat("YSens") + 1);
        PlayerPrefs.Save();
    }
    public void SensYUp5()
    {
        PlayerPrefs.SetFloat("YSens", PlayerPrefs.GetFloat("YSens") + 5);
        PlayerPrefs.Save();
    }
    public void SensYDown()
    {
        PlayerPrefs.SetFloat("YSens", PlayerPrefs.GetFloat("YSens") - 1);
        PlayerPrefs.Save();
    }
    public void SensYDown5()
    {
        PlayerPrefs.SetFloat("YSens", PlayerPrefs.GetFloat("YSens") - 5);
        PlayerPrefs.Save();
    }

    public void FOVUp()
    {
        PlayerPrefs.SetFloat("FOV", PlayerPrefs.GetFloat("FOV") + 1);
        PlayerPrefs.Save();
    }
    public void FOVUp5()
    {
        PlayerPrefs.SetFloat("FOV", PlayerPrefs.GetFloat("FOV") + 5);
        PlayerPrefs.Save();
    }
    public void FOVDown()
    {
        PlayerPrefs.SetFloat("FOV", PlayerPrefs.GetFloat("FOV") - 1);
        PlayerPrefs.Save();
    }
    public void FOVDown5()
    {
        PlayerPrefs.SetFloat("FOV", PlayerPrefs.GetFloat("FOV") - 5);
        PlayerPrefs.Save();
    }
    public void MainVolUp()
    {
        PlayerPrefs.SetFloat("MainVol", MathF.Round(PlayerPrefs.GetFloat("MainVol") + 0.1f, 1));
        for (int i = 0; i < allAudio.Length; i++)
        {
            allAudio[i].mute = false;
        }
        audioMixer.SetFloat("MainVolume", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("MainVol")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MainVolUp5()
    {
        PlayerPrefs.SetFloat("MainVol", MathF.Round(PlayerPrefs.GetFloat("MainVol") + 0.5f, 1));
        for (int i = 0; i < allAudio.Length; i++)
        {
            allAudio[i].mute = false;
        }
        audioMixer.SetFloat("MainVolume", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("MainVol")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MainVolDown()
    {
        PlayerPrefs.SetFloat("MainVol", MathF.Round(PlayerPrefs.GetFloat("MainVol") - 0.11f, 1));
        audioMixer.SetFloat("MainVolume", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("MainVol")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MainVolDown5()
    {
        PlayerPrefs.SetFloat("MainVol", MathF.Round(PlayerPrefs.GetFloat("MainVol") - 0.5f, 1));
        audioMixer.SetFloat("MainVolume", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("MainVol")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MusicUp()
    {
        PlayerPrefs.SetFloat("Music", MathF.Round(PlayerPrefs.GetFloat("Music") + 0.1f, 1));
        for (int i = 0; i < music.Length; i++)
        {
            music[i].mute = false;
        }
        audioMixer.SetFloat("Music", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MusicUp5()
    {
        PlayerPrefs.SetFloat("Music", MathF.Round(PlayerPrefs.GetFloat("Music") + 0.5f, 1));
        for (int i = 0; i < music.Length; i++)
        {
            music[i].mute = false;
        }
        audioMixer.SetFloat("Music", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MusicDown()
    {
        PlayerPrefs.SetFloat("Music", MathF.Round(PlayerPrefs.GetFloat("Music") - 0.1f, 1));
        audioMixer.SetFloat("Music", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void MusicDown5()
    {
        PlayerPrefs.SetFloat("Music", MathF.Round(PlayerPrefs.GetFloat("Music") - 0.5f, 1));
        audioMixer.SetFloat("Music", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Music")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void SoundUp()
    {
        PlayerPrefs.SetFloat("Sound", MathF.Round(PlayerPrefs.GetFloat("Sound") + 0.1f, 1));
        for (int i = 0; i < sound.Length; i++)
        {
            sound[i].mute = false;
        }
        audioMixer.SetFloat("Sound", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void SoundUp5()
    {
        PlayerPrefs.SetFloat("Sound", MathF.Round(PlayerPrefs.GetFloat("Sound") + 0.5f, 1));
        for (int i = 0; i < sound.Length; i++)
        {
            sound[i].mute = false;
        }
        audioMixer.SetFloat("Sound", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void SoundDown()
    {
        PlayerPrefs.SetFloat("Sound", MathF.Round(PlayerPrefs.GetFloat("Sound") - 0.1f, 1));
        audioMixer.SetFloat("Sound", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20, 1));
        PlayerPrefs.Save();
    }
    public void SoundDown5()
    {
        PlayerPrefs.SetFloat("Sound", MathF.Round(PlayerPrefs.GetFloat("Sound") - 0.5f, 1));
        audioMixer.SetFloat("Sound", MathF.Round(Mathf.Log10(PlayerPrefs.GetFloat("Sound")) * 20, 1));
        PlayerPrefs.Save();
    }
}
