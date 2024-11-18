using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpDownState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        ss.upSpace = new bool[ss.spawnSpots.Length];
        ss.downSpace = new bool[ss.spawnSpots.Length];

        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
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
