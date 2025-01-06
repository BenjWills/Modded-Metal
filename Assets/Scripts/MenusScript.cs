using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    Scene currentScene;

    PlayerInput playerInput;
    InputAction openMenuAction;
    bool menuOpen = false;

    GameObject pauseCanvas;
    GameObject mainCanvas;
    GameObject startCanvas;

    SlotMachine slotMachineScript;
    RespawnScript respawnScript;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);

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
    }

    public void PlayGame()
    {
        SceneManager.LoadScene("GameScene");
    }

    public void Settings()
    {

    }

    public void QuitGame()
    {
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
    }

    public void RSButton()
    {
        slotMachineScript.RemoveStats();
    }
}
