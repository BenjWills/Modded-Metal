using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] spawnSpots;
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject[] obstacles;
    [SerializeField] GameObject[] activeObstacles;
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
    Quaternion down;
    Quaternion left;
    Quaternion right;   
    Quaternion forward;
    Quaternion backward;

    public BaseSpawnerState currentState;
    public readonly UpDownState uds = new();
    public readonly SpawnUpDownState suds = new();
    public readonly HieghtCheckState hcs = new();
    public readonly CheckSdesState css = new();
    public readonly SpawnSidesState sss = new();


    // Start is called before the first frame update
    void Start()
    {
        spawnSpots = GameObject.FindGameObjectsWithTag("Spawnable");
        up = Quaternion.Euler(Vector3.up);
        down = Quaternion.Euler(Vector3.down);
        left = Quaternion.Euler(Vector3.left);
        right = Quaternion.Euler(Vector3.right);
        forward = Quaternion.Euler(Vector3.forward);
        backward = Quaternion.Euler(Vector3.back);

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
}
