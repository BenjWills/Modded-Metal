using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyCollision : MonoBehaviour
{
    RespawnScript respawnScript;
    GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        parentObject = GameObject.Find("obstacle 3(Clone)");
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawnScript.RespawnPlayer();
            Destroy(parentObject);
        }
    }
}
