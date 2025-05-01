using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WinScript : MonoBehaviour
{
    RespawnScript respawnScript;
    SpawnLevel spawnLevel;
    GameObject player;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        spawnLevel = GameObject.Find("SpawnLevel").GetComponent<SpawnLevel>();
        player = GameObject.Find("Player");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            PlayerPrefs.SetInt("winTotal", PlayerPrefs.GetInt("winTotal") + 1);
            PlayerPrefs.SetInt("smCoin", PlayerPrefs.GetInt("smCoin") + 1);
            player.transform.position = respawnScript.respawnPoint.position;
            spawnLevel.DespawnLevel();
        }
    }
}
