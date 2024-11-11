using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SlotMachine : MonoBehaviour
{
    string[] buff1;
    string[] buff2;
    string[] debuff;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(transform.position, 3, Vector3.forward, out RaycastHit hit))
        {
            if (hit.collider.gameObject.CompareTag("Player"))
            {
                RandomStats();
            }
        }
    }

    void RandomStats()
    {

    }
}
