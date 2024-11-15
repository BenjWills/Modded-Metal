using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSdesState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.right, out ss.hit, 1) == false)
            {
                ss.rightSpace[i] = true;
            }
            if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.left, out ss.hit, 1) == false)
            {
                ss.leftSpace[i] = true;
            }
            if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.forward, out ss.hit, 1) == false)
            {
                ss.forwardSpace[i] = true;
            }
            if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.back, out ss.hit, 1) == false)
            {
                ss.backwardSpace[i] = true;
            }
        }
    }

    public override void UpdateState(SpawnerScript ss)
    {

    }
}
