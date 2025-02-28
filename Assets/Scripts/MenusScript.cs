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

    // Start is called before the first frame update
    void Start()
    {
        SetObjects();
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
        }
    }

    void SetObjects()
    {
        settingsScript = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
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
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {
        settingsScript.settingsCanvas.SetActive(true);
        settingsScript.SetUi();
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
