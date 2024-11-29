using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenusScript : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction openMenuAction;
    bool menuOpen = false;

    [SerializeField] GameObject pauseCanvas;
    [SerializeField] GameObject mainCanvas;

    SlotMachine slotMachine;
    RespawnScript respawnScript;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        openMenuAction = playerInput.actions.FindAction("OpenMenu");
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        player = GameObject.Find("Player").GetComponent<Transform>();
        slotMachine = GameObject.Find("Slot Machine").GetComponent<SlotMachine>();
    }

    // Update is called once per frame
    void Update()
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
