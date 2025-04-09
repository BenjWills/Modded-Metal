using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotAnimation : MonoBehaviour
{
    Enemy1Script robot1Script;

    // Start is called before the first frame update
    void Start()
    {
        robot1Script = transform.parent.gameObject.GetComponent<Enemy1Script>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void RobotMove()
    {
        Debug.Log("Move2");
        robot1Script.EnemyMove();
    }

     public void RobotDie()
    {
        Debug.Log("Die2");
        robot1Script.EnemyDie();
    }
}
