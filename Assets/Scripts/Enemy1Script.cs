using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class Enemy1Script : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float flightSpeed;
    public RaycastHit hit;
    Collider[] colliders; 
    RespawnScript respawnScript;
    bool playerLockOn;
    [SerializeField] int destructionTimeInt;
    bool startLookTimer = true;
    Animator animator;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        player = GameObject.Find("Player");
        animator = GameObject.Find("Robot 1").GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //Physics.SphereCast(this.transform.position, 5, Vector3.forward, out hit);
        colliders = Physics.OverlapSphere(transform.position, 5);

        for (int i = 0; i < colliders.Length; i++)
        {
            if (colliders[i].gameObject.CompareTag("Player"))
            {
                playerLockOn = true;
            }
            else
            {
                playerLockOn = false;
            }
        }

        if (playerLockOn == true)
        {
            if (startLookTimer == true)
            {
                transform.LookAt(player.transform.position);
                startLookTimer = false;
            }
            animator.SetTrigger("Attack");
            transform.position += transform.forward * flightSpeed * Time.deltaTime;
            StartCoroutine(DestructionTimer());
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 5);
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
}
