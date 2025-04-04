using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float flightSpeed;
    RaycastHit phit;
    GameObject raycastHit;
    RespawnScript respawnScript;
    bool timeDone;

    [SerializeField] int destructionTimeInt;
    Transform startPos;
    bool startLookTimer = true;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        player = GameObject.Find("Player");
        startPos = this.transform;
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(this.transform.position, player.transform.position, out phit, 10);

        if (playerLockOn() == true)
        {
            if (startLookTimer == true)
            {
                transform.LookAt(player.transform.position);
                startLookTimer = false;
            }

            transform.position += transform.forward * flightSpeed * Time.deltaTime;
            StartCoroutine(DestructionTimer());
        }
    }

    bool playerLockOn()
    {
        if (phit.collider.gameObject.CompareTag("Player"))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawnScript.RespawnPlayer();
            Destroy(this.gameObject);
        }
    }

    IEnumerator DestructionTimer()
    {
        yield return new WaitForSeconds(destructionTimeInt);
        this.gameObject.SetActive(false);
    }

    IEnumerator LookAtTimer()
    {
        startLookTimer = true;
        transform.LookAt(player.transform.position);
        yield return new WaitForSeconds(0.1f);
    }
}
