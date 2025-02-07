using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        ss.RemoveObstacles();

        int spawnSpotsAmount = GameObject.FindGameObjectsWithTag("Spawnable").Length;
        ss.spawnSpots = new GameObject[spawnSpotsAmount];
        ss.spawnSpots = GameObject.FindGameObjectsWithTag("Spawnable");
        ss.sr = new SpawningRestriction[ss.spawnSpots.Length];

        ss.spawnDone = false;
        ss.reset = false;

        ss.upSpace = new bool[ss.spawnSpots.Length];
        ss.downSpace = new bool[ss.spawnSpots.Length];

        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            ss.sr[i] = ss.spawnSpots[i].GetComponent<SpawningRestriction>();
            if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.up, out ss.hit, 2) == false)
            {
                ss.upSpace[i] = true;
            }
            if (Physics.Raycast(ss.spawnSpots[i].transform.position - ss.spawnSpots[i].transform.localScale / 2, Vector3.down, out ss.hit, 2) == false)
            {
                ss.downSpace[i] = true;
            }
        }
        ss.checkDone = true;
    }

    public override void UpdateState(SpawnerScript ss)
    {
        if (ss.checkDone == true)
        {
            ss.TransitionToSate(ss.suds);
        }
    }
}
