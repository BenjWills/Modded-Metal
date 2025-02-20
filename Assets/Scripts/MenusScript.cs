using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;
using System;

public class MenusScript : MonoBehaviour
{
    public static MenusScript _instance;
    public static MenusScript Instance { get { return _instance; } }

    Scene currentScene;

    PlayerInput playerInput;
    InputAction openMenuAction;
    bool menuOpen = false;

    GameObject pauseCanvas;
    GameObject mainCanvas;
    GameObject startCanvas;
    [SerializeField] GameObject settingsCanvas;

    Toggle fullscreenToggle;
    Slider brightnessSlider;
    TMP_Dropdown rDropdown;

    bool objectsSet;

    SlotMachine slotMachineScript;
    RespawnScript respawnScript;
    Settings settingsScript;
    Transform player;

    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI wins;
    [SerializeField] TextMeshProUGUI deaths;
    [SerializeField] TextMeshProUGUI levelsSpawned;

    Resolution[] resolutions;
    List<Resolution> filteredResolutions;
    double currentRefreshRate;
    int currentResolutionIndex;

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
    }

    // Start is called before the first frame update
    void Start()
    {
        SetObjects();

        resolutions = Screen.resolutions;
        filteredResolutions = new List<Resolution>();
        rDropdown.ClearOptions();
        currentRefreshRate = Screen.currentResolution.refreshRateRatio.value;

        for (int i = 0; i < resolutions.Length; i++)
        {
            Debug.Log(resolutions[i]);
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

    // Update is called once per frame
    void Update()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "GameScene")
        {
            playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
            player = GameObject.Find("Player").GetComponent<Transform>();

            openMenuAction = playerInput.actions.FindAction("OpenMenu");

            mainCanvas.SetActive(!pauseCanvas.activeSelf);
            pauseCanvas.SetActive(menuOpen);
            Time.timeScale = mainCanvas.activeSelf ? 1 : 0;

            if (openMenuAction.triggered)
            {
                menuOpen = !menuOpen;
            }
            if (menuOpen == true)
            {
                Cursor.lockState = CursorLockMode.None;
            }
            else
            {
                Cursor.lockState = CursorLockMode.Locked;
            }
            coins.text = "Coins: " + PlayerPrefs.GetInt("smCoin").ToString();
            wins.text = "Wins: " + PlayerPrefs.GetInt("winTotal").ToString();
            deaths.text = "Deaths: " + PlayerPrefs.GetInt("deathTotal").ToString();
            levelsSpawned.text = "Levels Spawned: " + PlayerPrefs.GetInt("levelsSpawned").ToString();

            SetSettings();
        }
    }

    void SetObjects()
    {
        GameObject[] goArray = Resources.FindObjectsOfTypeAll<GameObject>();
        while (objectsSet == false)
        {
            for (int i = 0; i < goArray.Length; i++)
            {
                switch (goArray[i].name)
                {
                    case "StartCanvas":
                        startCanvas = goArray[i];
                        break;
                    case "Settings Values":
                        settingsScript = goArray[i].GetComponent<Settings>();
                        break;
                    case "MainCanvas":
                        mainCanvas = goArray[i];
                        break;
                    case "PauseCanvas":
                        pauseCanvas = goArray[i];
                        break;
                    case "Respawn Point":
                        respawnScript = goArray[i].GetComponent<RespawnScript>();
                        break;
                    case "Slot Machine":
                        slotMachineScript = goArray[i].GetComponent<SlotMachine>();
                        break;
                    case "Fullscreen Toggle":
                        fullscreenToggle = goArray[i].GetComponent<Toggle>();
                        break;
                    case "Brightness Slider":
                        brightnessSlider = goArray[i].GetComponent<Slider>();
                        break;
                    case "Resolution Dropdown":
                        rDropdown = goArray[i].GetComponent<TMP_Dropdown>();
                        break;
                }
            }
            objectsSet = true;
        }
        if (PlayerPrefs.HasKey("Brightness") && currentScene.name == "MainMenu")
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
        }
        if (PlayerPrefs.HasKey("Fullscreen") && currentScene.name == "MainMenu")
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

        SetSettings();
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

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        settingsCanvas.SetActive(!settingsCanvas.activeSelf);
    }

    public void QuitGame()
    {
        PlayerPrefs.Save();
        Application.Quit();
    }

    public void PauseBack()
    {
        menuOpen = false;
    }

    public void FullscreenToggle()
    {
        Screen.fullScreen = fullscreenToggle.isOn;
        PlayerPrefs.SetInt("Fullscreen", (Screen.fullScreen ? 1 : 0));
        PlayerPrefs.Save();
        Debug.Log(fullscreenToggle.isOn);
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

    public void MainMenu()
    {
        SceneManager.LoadScene("MainMenu");
    }

    public void RespawnPlayer()
    {
        PlayerPrefs.SetInt("deathTotal", PlayerPrefs.GetInt("deathTotal") + 1);
        player.position = respawnScript.respawnPoint.position;
        PlayerPrefs.Save();
    }

    public void RSButton()
    {
        slotMachineScript.RemoveStats();
    }
}
