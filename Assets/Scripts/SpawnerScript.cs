using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] spawnSpots;
    [SerializeField] GameObject obstacle;
    [SerializeField] GameObject[] obstacles;
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

    public void ResetObstacle()
    {
        reset = true;
        StartCoroutine(ResetWaitTime());
    }

    public void SpawnUp(Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnDown(Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnLeft(Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnRight(Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnForward(Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnBack(Vector3 vector)
    {
        obstacle = obstacles[Random.Range(0, obstacles.Length)];
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }

    IEnumerator ResetWaitTime()
    {
        yield return new WaitForEndOfFrame();
        reset = false;
    }
}
