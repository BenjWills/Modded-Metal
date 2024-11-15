using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    public GameObject[] spawnSpots;
    [SerializeField] GameObject obstacle;
    public bool[] upSpace;
    public bool[] downSpace;
    public bool[] leftSpace;
    public bool[] rightSpace;
    public bool[] forwardSpace;
    public bool[] backwardSpace;
    public bool[] hieghtBigger;
    public bool checkDone;
    public bool spawnDone;
    public RaycastHit hit;
    public float playerHeight;

    public BaseSpawnerState currentState;
    public readonly UpDownState uds = new();
    public readonly SpawnUpDownState suds = new();
    public readonly HieghtCheckState hcs = new();
    public readonly CheckSdesState css = new();


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

    public void SpawnUp(Vector3 vector)
    {
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnDown(Vector3 vector)
    {
        Instantiate(obstacle, vector, obstacle.transform.rotation);
    }
    public void SpawnLeft()
    {

    }
    public void SpawnRight()
    {

    }
    public void SpawnForward()
    {

    }
    public void SpawnBack()
    {

    }
}
