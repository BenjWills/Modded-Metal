using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using TMPro;

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

    SlotMachine slotMachineScript;
    RespawnScript respawnScript;
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
            startCanvas = GameObject.Find("StartCanvas");
        }
        if (currentScene.name == "GameScene")
        {
            mainCanvas = GameObject.Find("MainCanvas");
            pauseCanvas = GameObject.Find("PauseCanvas");
            playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
            respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
            player = GameObject.Find("Player").GetComponent<Transform>();
            slotMachineScript = GameObject.Find("Slot Machine").GetComponent<SlotMachine>();
            openMenuAction = playerInput.actions.FindAction("OpenMenu");
        }
    }

    // Update is called once per frame
    void Update()
    {
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
        }

        coins.text = "Coins: " + PlayerPrefs.GetInt("smCoin").ToString();
        wins.text = "Wins: " + PlayerPrefs.GetInt("winTotal").ToString();
        deaths.text = "Deaths: " + PlayerPrefs.GetInt("deathTotal").ToString();
        levelsSpawned.text = "Levels Spawned: " + PlayerPrefs.GetInt("levelsSpawned").ToString();
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
