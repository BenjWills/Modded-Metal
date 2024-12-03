using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SpawnLevel : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction interactAction;
    SphereCollider buttonRange;
    private bool inTrigger;
    [SerializeField] GameObject[] levelArray;
    SpawnerScript spawnerScript;
    [SerializeField] Transform levelPos;

    private void Awake()
    {
        spawnerScript = GameObject.Find("StuffSpawner").GetComponent<SpawnerScript>();

        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");

        buttonRange = this.gameObject.AddComponent<SphereCollider>();
        buttonRange.radius = 1.2f;
        buttonRange.isTrigger = true;
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
            if (interactAction.triggered)
            {
                DespawnLevel();
                GenerateLevel();
            }
        }
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
            Instantiate(levelArray[Random.Range(0, levelArray.Length)]);
            spawnerScript.StartLevelSpawning();
        }
    }
    private void DespawnLevel()
    {
        GameObject currentLevel = GameObject.FindGameObjectWithTag("Level");
        if (currentLevel != null)
        {
            spawnerScript.RemoveObstacles();
            Destroy(currentLevel);
        }
    }
}
