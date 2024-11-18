using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CheckSdesState : BaseSpawnerState
{
    public override void EnterState(SpawnerScript ss)
    {
        ss.checkDone = false;
        ss.forwardSpace = new bool[ss.spawnSpots.Length];
        ss.backwardSpace = new bool[ss.spawnSpots.Length];
        ss.rightSpace = new bool[ss.spawnSpots.Length];
        ss.leftSpace = new bool[ss.spawnSpots.Length];

        for (int i = 0; i < ss.spawnSpots.Length; i++)
        {
            if (ss.zBig[i] == true)
            {
                if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.forward, out ss.hit, 2) == false)
                {
                    ss.forwardSpace[i] = true;
                }
                if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.back, out ss.hit, 2) == false)
                {
                    ss.backwardSpace[i] = true;
                }
            }
            if (ss.xBig[i] == true)
            {
                if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.right, out ss.hit, 2) == false)
                {
                    ss.rightSpace[i] = true;
                }
                if (Physics.Raycast(ss.spawnSpots[i].transform.position + ss.spawnSpots[i].transform.localScale / 2, Vector3.left, out ss.hit, 2) == false)
                {
                    ss.leftSpace[i] = true;
                }
            }
        }
        ss.checkDone = true;
    }

    public override void UpdateState(SpawnerScript ss)
    {
        if (ss.checkDone == true)
        {
            ss.TransitionToSate(ss.sss);
        }
    }
}
