using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSidesState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {

            if (ss.forwardSpace[i] == true && ss.sr[i].forward == true)
            {
                Vector3 tempVectorF = ss.RandomPosition("forward", ss.spawnSpots[i], ss.randomX[i], ss.randomY[i], ss.randomZ[i]);
                ss.SpawnObstacle("forward",tempVectorF);
            }
            if (ss.backwardSpace[i] == true && ss.sr[i].backward == true)
            {
                Vector3 tempVectorB = ss.RandomPosition("back", ss.spawnSpots[i], ss.randomX[i], ss.randomY[i], ss.randomZ[i]);
                ss.SpawnObstacle("backward", tempVectorB);
            }
            if (ss.leftSpace[i] == true && ss.sr[i].left == true)
            {
                Vector3 tempVectorL = ss.RandomPosition("left", ss.spawnSpots[i], ss.randomX[i], ss.randomY[i], ss.randomZ[i]);
                ss.SpawnObstacle("left", tempVectorL);
            }
            if (ss.rightSpace[i] == true && ss.sr[i].right == true)
            {
                Vector3 tempVectorR = ss.RandomPosition("right", ss.spawnSpots[i], ss.randomX[i], ss.randomY[i], ss.randomZ[i]);
                ss.SpawnObstacle("right", tempVectorR);
            }
        }
        ss.spawnDone = true;
    }

    public override void UpdateState(SpawnerScript ss)
    {
        if (ss.spawnDone == true && ss.reset == true)
        {
            ss.TransitionToSate(ss.uds);
        }
    }
}
