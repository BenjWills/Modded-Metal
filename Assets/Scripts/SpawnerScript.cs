using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] spawnSpots;
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject[] obstacles;
    GameObject[] activeObstacles;
    public SpawningRestriction[] sr;
    public bool[] upSpace;
    public bool[] downSpace;
    public bool[] leftSpace;
    public bool[] rightSpace;
    public bool[] forwardSpace;
    public bool[] backwardSpace;
    public bool[] zBig;
    public bool[] xBig;
    public float[] randomX;
    public float[] randomY;
    public float[] randomZ;
    public bool checkDone;
    public bool spawnDone;
    public bool reset;
    public RaycastHit hit;
    public float playerHeight;

    [SerializeField] int spawnChance;
    int spawnNumber;
    Quaternion up;

    public BaseSpawnerState currentState;
    public readonly UpDownState uds = new();
    public readonly SpawnUpDownState suds = new();
    public readonly HieghtCheckState hcs = new();
    public readonly CheckSdesState css = new();
    public readonly SpawnSidesState sss = new();


    // Start is called before the first frame update
    void Start()
    {
        up = Quaternion.Euler(Vector3.up);
        TransitionToSate(uds);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdateState(this);
    }

    public void TransitionToSate(BaseSpawnerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }

    public void StartLevelSpawning()
    {
        ResetBool();
    }

    public void ResetBool()
    {
        reset = true;
    }

    public void RemoveObstacles()
    {
        activeObstacles = GameObject.FindGameObjectsWithTag("Obstacle");
        for (int i = 0; i < activeObstacles.Length; i++)
        {
            Destroy(activeObstacles[i]);
        }
    }

    public void SpawnObstacle(string direction, Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        spawnNumber = Random.Range(0, spawnChance);
        if (spawnNumber == spawnChance - 1)
        {
            GameObject newSpawn = Instantiate(obstacle, vector, up);
            newSpawn.transform.up = CheckForDirection(direction, newSpawn);
        }
    }

    public Vector3 CheckForDirection(string direction, GameObject newSpawn)
    {
        switch(direction)
        {
            default:
                return newSpawn.transform.up;
            case "down":
                return -newSpawn.transform.up;
            case "left":
                return -newSpawn.transform.right;
            case "right":
                return newSpawn.transform.right;
            case "forward":
                return newSpawn.transform.forward;
            case "backward":
                return -newSpawn.transform.forward;
        }
    }

    public Vector3 RandomPosition(string position, GameObject spawnObject, float randomValueX, float randomValueY, float randomValueZ)
    {
        switch (position)
        {
            default :
                return new Vector3(spawnObject.transform.position.x + randomValueX, spawnObject.transform.position.y + randomValueY, (spawnObject.transform.position.z + (spawnObject.transform.localScale.z/2)) + 0.5f);
            case "back":
                return new Vector3(spawnObject.transform.position.x + randomValueX, spawnObject.transform.position.y + randomValueY, (spawnObject.transform.position.z - (spawnObject.transform.localScale.z / 2)) - 0.5f);
            case "left":
                return new Vector3((spawnObject.transform.position.x - (spawnObject.transform.localScale.x / 2)) - 0.5f, spawnObject.transform.position.y + randomValueY, spawnObject.transform.position.z + randomValueZ);
            case "right":
                return new Vector3((spawnObject.transform.position.x + (spawnObject.transform.localScale.x / 2)) + 0.5f, spawnObject.transform.position.y + randomValueY, spawnObject.transform.position.z + randomValueZ);
        }
    }
}
