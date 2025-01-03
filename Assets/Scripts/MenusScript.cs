using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    GameObject[] gObjects;
    Scene currentScene;

    GameObject playerGO;
    PlayerInput playerInput;
    InputAction openMenuAction;
    bool menuOpen = false;

    GameObject pauseCanvas;
    GameObject mainCanvas;
    GameObject startCanvas;

    GameObject slotMachine;
    SlotMachine slotMachineScript;
    GameObject respawnPoint;
    RespawnScript respawnScript;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        gObjects = Resources.FindObjectsOfTypeAll<GameObject>();
        currentScene = SceneManager.GetActiveScene();
        Debug.Log(currentScene.name);

        if (currentScene.name == "MainMenu")
        {
            for (int i = 0; i < gObjects.Length; i++)
            {
                SetObject(startCanvas, gObjects[i]);
            }
        }
        if (currentScene.name == "GameScene")
        {
            for (global::System.Int32 i = 0; i < gObjects.Length; i++)
            {
                SetObject(mainCanvas, gObjects[i]);
                SetObject(pauseCanvas, gObjects[i]);
                SetObject(playerGO, gObjects[i]);
                SetObject(respawnPoint, gObjects[i]);
                SetObject(slotMachine, gObjects[i]);
            }

            playerInput = playerGO.GetComponent<PlayerInput>();
            respawnScript = respawnPoint.GetComponent<RespawnScript>();
            player = playerGO.GetComponent<Transform>();
            slotMachineScript = slotMachine.GetComponent<SlotMachine>();
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

    void SetObject(GameObject currentgo, GameObject gobject)
    {
        switch (gobject.name)
        {
            default:
                Debug.Log(gobject.name + " was not set!");
                break;
            case "PauseCanvas":
                currentgo = gobject;
                break;
            case "MainCanvas":
                currentgo = gobject;
                break;
            case "Player":
                currentgo = gobject;
                break;
            case "Respawn Point":
                currentgo = gobject;
                break;
            case "Slot Machine":
                currentgo = gobject;
                break;
            case "StartCanvas":
                currentgo = gobject;
                break;
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

    public void RespawnPlayer()
    {
        player.position = respawnScript.respawnPoint.position;
    }

    public void RSButton()
    {
        slotMachineScript.RemoveStats();
    }
}
