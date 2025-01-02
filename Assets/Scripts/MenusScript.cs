using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    GameObject[] gObjects;
    GameObject go;
    bool objectsSet = false;

    PlayerInput playerInput;
    InputAction openMenuAction;
    bool menuOpen = false;

    GameObject pauseCanvas;
    GameObject mainCanvas;
    GameObject startCanvas;

    SlotMachine slotMachine;
    RespawnScript respawnScript;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        gObjects = Resources.FindObjectsOfTypeAll<GameObject>();

        playerInput = SetObject(go.name).GetComponent<PlayerInput>();
        Debug.Log(go.name);
        openMenuAction = playerInput.actions.FindAction("OpenMenu");
        respawnScript = SetObject(go.name).GetComponent<RespawnScript>();
        Debug.Log(go.name);
        player = SetObject(go.name).GetComponent<Transform>();
        slotMachine = SetObject(go.name).GetComponent<SlotMachine>();
        mainCanvas = SetObject(go.name);
        pauseCanvas = SetObject(go.name);
        startCanvas = SetObject(go.name);
        if (startCanvas != null)
        {
            objectsSet = true;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (objectsSet == false)
        {
            FindObject();
        }
        Scene currentScene = SceneManager.GetActiveScene();

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

    GameObject SetObject(string objectName)
    {
        switch (objectName)
        {
            default:
                return null;
            case "PauseCanvas":
                return go;
            case "MainCanvas":
                return go;
            case "Player":
                return go;
            case "Respawn Point":
                return go;
            case "Slot Machine":
                return go;            
            case "StartCanvas":
                return go;
        }
    }

    void FindObject()
    {
        for (int i = 0; i < gObjects.Length; i++)
        {
            go = gObjects[i];
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
        slotMachine.RemoveStats();
    }
}
