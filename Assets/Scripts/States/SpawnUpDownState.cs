using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUpDownState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        ss.randomX = new float[ss.spawnSpots.Length];
        ss.randomY = new float[ss.spawnSpots.Length];
        ss.randomZ = new float[ss.spawnSpots.Length];

        ss.checkDone = false;
        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            ss.randomX[i] = Random.Range(-ss.spawnSpots[i].transform.localScale.x / 2 + 0.5f, ss.spawnSpots[i].transform.localScale.x / 2 - 0.5f);
            ss.randomY[i] = Random.Range(-ss.spawnSpots[i].transform.localScale.y / 2 + 0.5f, ss.spawnSpots[i].transform.localScale.y / 2 - 0.5f);
            ss.randomZ[i] = Random.Range(-ss.spawnSpots[i].transform.localScale.z / 2 + 0.5f, ss.spawnSpots[i].transform.localScale.z / 2 - 0.5f);
            if (ss.upSpace[i] == true)
            {
                Vector3 tempVectorU = new Vector3(ss.spawnSpots[i].transform.position.x + ss.randomX[i], ss.spawnSpots[i].transform.position.y + ss.spawnSpots[i].transform.localScale.y / 2 + 0.5f, ss.spawnSpots[i].transform.position.z + ss.randomZ[i]);
                ss.SpawnObstacle("up", tempVectorU);
            }
            if (ss.downSpace[i] == true)
            {
                Vector3 tempVectorD = new Vector3(ss.spawnSpots[i].transform.position.x + ss.randomX[i], ss.spawnSpots[i].transform.position.y - ss.spawnSpots[i].transform.localScale.y / 2 - 0.5f, ss.spawnSpots[i].transform.position.z + ss.randomZ[i]);
                ss.SpawnObstacle("down", tempVectorD);
            }
        }
        ss.spawnDone = true;
    }
    public override void UpdateState(SpawnerScript ss)
    {
        if (ss.spawnDone == true)
        {
            ss.TransitionToSate(ss.hcs);
        }
    }
}
