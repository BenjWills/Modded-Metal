using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnUpDownState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            if (ss.upSpace[i] == true)
            {
                Vector3 tempVectorU = new Vector3(ss.spawnSpots[i].transform.position.x, ss.spawnSpots[i].transform.position.y + ss.spawnSpots[i].transform.localScale.y / 2 + 0.5f, ss.spawnSpots[i].transform.position.z);
                ss.SpawnUp(tempVectorU);
            }
            if (ss.downSpace[i] == true)
            {
                Vector3 tempVectorD = new Vector3(ss.spawnSpots[i].transform.position.x, ss.spawnSpots[i].transform.position.y - ss.spawnSpots[i].transform.localScale.y / 2 - 0.5f, ss.spawnSpots[i].transform.position.z);
                ss.SpawnDown(tempVectorD);
            }
        }
    }
    public override void UpdateState(SpawnerScript ss)
    {

    }
}
