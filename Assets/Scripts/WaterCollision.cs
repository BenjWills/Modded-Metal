using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WaterCollision : MonoBehaviour
{
    RespawnScript respawnScript;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            respawnScript.RespawnPlayer();
        }
    }
}
