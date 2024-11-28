using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] float lowestLevel;
    GameObject player;
    float playerHeight;
    public Transform respawnPoint;

    private void Awake()
    {
        player = GameObject.Find("Player");
        playerHeight = player.transform.position.y;
        respawnPoint = this.gameObject.GetComponent<Transform>();
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        playerHeight = player.transform.position.y;
        if (lowestLevel >= playerHeight)
        {
            player.transform.position = respawnPoint.position;
        }
    }
}
