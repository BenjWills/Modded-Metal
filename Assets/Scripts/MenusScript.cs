using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;
using UnityEngine.UI;

public class MenusScript : MonoBehaviour
{
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

    bool bValueSet;
    bool fValueSet;

    SlotMachine slotMachineScript;
    RespawnScript respawnScript;
    Settings settingsScript;
    Transform player;

    [SerializeField] TextMeshProUGUI coins;
    [SerializeField] TextMeshProUGUI wins;
    [SerializeField] TextMeshProUGUI deaths;
    [SerializeField] TextMeshProUGUI levelsSpawned;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();

        if (currentScene.name == "MainMenu")
        {
            settingsScript = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
            startCanvas = GameObject.Find("StartCanvas");
        }
        if (currentScene.name == "GameScene")
        {
            settingsScript = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
            mainCanvas = GameObject.Find("MainCanvas");
            pauseCanvas = GameObject.Find("PauseCanvas");
            playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
            respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
            player = GameObject.Find("Player").GetComponent<Transform>();
            slotMachineScript = GameObject.Find("Slot Machine").GetComponent<SlotMachine>();
            openMenuAction = playerInput.actions.FindAction("OpenMenu");
            fullscreenToggle = GameObject.Find("Fullscreen Toggle").GetComponent<Toggle>();
        }
        SetSettings();
    }

    // Update is called once per frame
    void Update()
    {
        if (settingsCanvas.activeSelf == true)
        {
            fullscreenToggle = GameObject.Find("Fullscreen Toggle").GetComponent<Toggle>();
            brightnessSlider = GameObject.Find("Brightness Slider").GetComponent<Slider>();
            if (PlayerPrefs.HasKey("Brightness") && bValueSet == false)
            {
                brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
                bValueSet = true;
            }
            if (PlayerPrefs.HasKey("Fullscreen") && fValueSet == false)
            {
                if (PlayerPrefs.GetInt("Fullscreen") != 0)
                {
                    fullscreenToggle.isOn = true;
                    fValueSet = true;
                }
                else
                {
                    fullscreenToggle.isOn = false;
                    fValueSet = true;
                }
            }
        }

        if (currentScene.name == "GameScene")
        {
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
