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

    RespawnScript respawnScript;
    Transform player;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        openMenuAction = playerInput.actions.FindAction("OpenMenu");
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        player = GameObject.Find("Player").GetComponent<Transform>();
    }

    // Update is called once per frame
    void Update()
    {
        mainCanvas.SetActive(!pauseCanvas.activeSelf);
        pauseCanvas.SetActive(menuOpen);
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
}
