using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnSidesState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            if (ss.forwardSpace[i] == true)
            {
                Vector3 tempVectorF = new Vector3(ss.spawnSpots[i].transform.position.x + ss.randomX[i], ss.spawnSpots[i].transform.position.y + ss.randomY[i], ss.spawnSpots[i].transform.position.z + ss.spawnSpots[i].transform.localScale.z / 2 + 0.5f);
                ss.SpawnForward(tempVectorF);
            }
            if (ss.backwardSpace[i] == true)
            {
                Vector3 tempVectorB = new Vector3(ss.spawnSpots[i].transform.position.x + ss.randomX[i], ss.spawnSpots[i].transform.position.y + ss.randomY[i], ss.spawnSpots[i].transform.position.z - ss.spawnSpots[i].transform.localScale.z / 2 - 0.5f);
                ss.SpawnBack(tempVectorB);
            }
            if (ss.leftSpace[i] == true)
            {
                Vector3 tempVectorB = new Vector3(ss.spawnSpots[i].transform.position.x - ss.spawnSpots[i].transform.localScale.x / 2 - 0.5f, ss.spawnSpots[i].transform.position.y + ss.randomY[i], ss.spawnSpots[i].transform.position.z + ss.randomZ[i]);
                ss.SpawnBack(tempVectorB);
            }
            if (ss.rightSpace[i] == true)
            {
                Vector3 tempVectorR = new Vector3(ss.spawnSpots[i].transform.position.x + ss.spawnSpots[i].transform.localScale.x / 2 + 0.5f, ss.spawnSpots[i].transform.position.y + ss.randomY[i], ss.spawnSpots[i].transform.position.z + ss.randomZ[i]);
                ss.SpawnForward(tempVectorR);
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
