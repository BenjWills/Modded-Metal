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
    [SerializeField] TextMeshProUGUI timerText;
    [SerializeField] TextMeshPro bestTimeText;
    public bool timerStarted;
    float timerTime;
    MenusScript menuScript;
    Settings settings;
    [SerializeField] BouncePad bouncePad;
    public DoorAnim doorAnim;
    AudioSource buttonClickAudio;
    int previousWins;
    GameObject currentLevel;
    public bool isSpawned;
    public bool isDespawned;

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
        buttonClickAudio = GetComponent<AudioSource>();
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (inTrigger == true)
        {
            menuScript.interactTxt.enabled = true;
            if (interactAction.triggered)
            {
                doorAnim.ButtonPush();
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
        buttonClickAudio.Play();
        currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (currentLevel == null)
        {
            isSpawned = true;
            StartCoroutine(SpawningLevel());
        }
    }

    public void DespawnLevel()
    {
        currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (currentLevel != null)
        {
            isDespawned = true;
            StartCoroutine(DespawningLevel());
        }
    }

    void Timer()
    {

        if (currentLevel != null && timerStarted == true)
        {
            timerTime += Time.deltaTime;
            timerText.text = timerTime.ToString();
        }
        else if (timerStarted == false) 
        {
            if (timerTime < PlayerPrefs.GetFloat("BestTime") && timerTime !=0 || PlayerPrefs.GetFloat("BestTime") == 0)
            {
                if (PlayerPrefs.GetInt("winTotal") > previousWins && PlayerPrefs.GetInt("winTotal") != 0)
                {
                    PlayerPrefs.SetFloat("BestTime", timerTime);
                    PlayerPrefs.Save();
                }
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
        yield return new WaitForEndOfFrame();
        GenerateLevel();
    }

    IEnumerator SpawningLevel()
    {
        while (isSpawned == true)
        {
            settings.MuteMusic();

            for (int i = 0; i < settings.music.Length; i++)
            {
                settings.music[i].Stop();
                settings.UnmuteMusic();
                settings.music[i].Play();
            }
            PlayerPrefs.SetInt("levelsSpawned", PlayerPrefs.GetInt("levelsSpawned") + 1);
            Instantiate(levelArray[Random.Range(0, levelArray.Length)], levelPos);
            spawnerScript.StartLevelSpawning();
            doorAnim.DoorOpen();
            PlayerPrefs.Save();
            previousWins = PlayerPrefs.GetInt("winTotal");
            isSpawned = false;
        }
        yield return null;
    }
    IEnumerator DespawningLevel()
    {
        while (isDespawned == true)
        {
            doorAnim.DoorClose();
            yield return new WaitForSeconds(1);
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
            spawnerScript.RemoveObstacles();
            //bouncePad.DestroyBouncePad();
            Destroy(currentLevel);
            timerStarted = false;
            isDespawned = false;
        }
        yield return null;
    }
}
