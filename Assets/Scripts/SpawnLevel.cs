using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEditor;

public class SpawnLevel : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction interactAction;
    SphereCollider buttonRange;
    private bool inTrigger;
    private bool inTriggerSet;
    [SerializeField] GameObject[] levelArray;
    SpawnerScript spawnerScript;
    [SerializeField] Transform levelPos;
    [SerializeField] GameObject levelDoor;
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshProUGUI bestTimeText;
    public bool timerStarted;
    float timerTime;
    MenusScript menuScript;
    Settings settings;
    [SerializeField] DoorAnim doorAnim;

    private void Awake()
    {
        spawnerScript = GameObject.Find("StuffSpawner").GetComponent<SpawnerScript>();
        settings = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();

        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");

        buttonRange = this.gameObject.AddComponent<SphereCollider>();
        buttonRange.radius = 1.2f;
        buttonRange.isTrigger = true;

        menuScript = GameObject.Find("Menus").GetComponent<MenusScript>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger == true)
        {
            menuScript.interactTxt.enabled = true;
            if (interactAction.triggered)
            {
                doorAnim.doorAnimator.SetTrigger("Door");
            }
        }
        else
        {
            menuScript.interactTxt.enabled = false;
        }
        Timer();
        bestTimeText.text = "Best: " + PlayerPrefs.GetFloat("BestTime").ToString();
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
        }
    }

    private void GenerateLevel()
    {
        GameObject currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (currentLevel == null)
        {
            settings.MuteMusic();
            for (int i = 0; i < settings.music.Length; i++)
            {
                settings.music[i].Stop();
                settings.UnmuteMusic();
                settings.music[i].Play();
            }
            PlayerPrefs.SetInt("levelsSpawned", PlayerPrefs.GetInt("levelsSpawned") + 1);
            levelDoor.SetActive(false);
            Instantiate(levelArray[Random.Range(0, levelArray.Length)], levelPos);
            spawnerScript.StartLevelSpawning();
            timerStarted = true;
            PlayerPrefs.Save();
        }
    }

    public void DespawnLevel()
    {
        GameObject currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (currentLevel != null)
        {
            settings.MuteMusic();

            for (int i = 0; i < settings.music.Length; i++)
            {
                settings.music[i].Stop();
                if (settings.music[i].gameObject.name == "Bass" || settings.music[i].gameObject.name == "Drums")
                {
                    settings.UnmuteMusic();
                    settings.music[i].Play();
                }
            }
            levelDoor.SetActive(true);
            spawnerScript.RemoveObstacles();
            Destroy(currentLevel);
            timerStarted = false;
        }
    }

    void Timer()
    {
        GameObject currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (currentLevel != null && timerStarted == true)
        {
            timerTime += Time.deltaTime;
            timerText.text = timerTime.ToString();
        }
        else if (timerStarted == false) 
        {
            if (timerTime < PlayerPrefs.GetFloat("BestTime"))
            {
                PlayerPrefs.SetFloat("BestTime", timerTime);
                PlayerPrefs.Save();
            }
            timerTime = 0;
            timerText.text = timerTime.ToString();
        }
    }

    public void SpawnTheLevel()
    {
        StartCoroutine(LevelSpawn());
    }

    IEnumerator LevelSpawn()
    {
        DespawnLevel();
        yield return new WaitForSeconds(0.1f);
        GenerateLevel();
    }

    
}
