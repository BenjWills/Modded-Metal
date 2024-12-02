using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HieghtCheckState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        ss.spawnDone = false;
        ss.zBig = new bool[ss.spawnSpots.Length];
        ss.xBig = new bool[ss.spawnSpots.Length];

        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            if (ss.spawnSpots[i].transform.localScale.y > 1.5 && ss.spawnSpots[i].transform.localScale.x > 1.5f)
            {
                ss.zBig[i] = true;
            }
            if (ss.spawnSpots[i].transform.localScale.y > 1.5 && ss.spawnSpots[i].transform.localScale.z > 1.5f)
            {
                ss.xBig[i] = true;
            }
        }
        ss.checkDone = true;
    }

    public override void UpdateState(SpawnerScript ss)
    {
        if (ss.checkDone == true)
        {
            ss.TransitionToSate(ss.css);
        }
    }
}
