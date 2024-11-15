using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HieghtCheckState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            if (ss.spawnSpots[i].transform.localScale.y > 1.5)
            {
                ss.hieghtBigger[i] = true;
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
