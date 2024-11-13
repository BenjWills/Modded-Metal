using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnerScript : MonoBehaviour
{
    [SerializeField] GameObject[] spawnSpots;
    [SerializeField] GameObject ground;

    public BaseSpawnerState currentState;
    public readonly UpDownState uds = new UpDownState();


    // Start is called before the first frame update
    void Start()
    {
        spawnSpots = GameObject.FindGameObjectsWithTag("Spawnable");
        ground = GameObject.Find("BaseGround");

        TransitionToSate(uds);
    }

    // Update is called once per frame
    void Update()
    {
        currentState.UpdaterState(this);
    }

    public void TransitionToSate(BaseSpawnerState state)
    {
        currentState = state;
        currentState.EnterState(this);
    }
}
