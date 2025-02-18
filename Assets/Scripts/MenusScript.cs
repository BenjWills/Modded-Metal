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

    bool bValueSet = false;
    bool fValueSet = false;
    bool objectsSet;

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

    }

    // Update is called once per frame
    void Update()
    {
        SetObjects();

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
        }
    }

    void SetObjects()
    {
        if (objectsSet == false)
        {
            GameObject[] goArray = Resources.FindObjectsOfTypeAll<GameObject>();
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
                }
            }

            objectsSet = true;
        }
        if (PlayerPrefs.HasKey("Brightness") && bValueSet == false && currentScene.name == "MainMenu")
        {
            brightnessSlider.value = PlayerPrefs.GetFloat("Brightness");
            bValueSet = true;
        }
        if (PlayerPrefs.HasKey("Fullscreen") && fValueSet == false && currentScene.name == "MainMenu")
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
