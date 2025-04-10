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
    bool startLook = true;
    Animator animator;
    bool doneOnce =false;
    bool moveRobot;

    SphereCollider sphereCollider;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        player = GameObject.Find("Player");
        animator = transform.GetChild(0).GetComponent<Animator>();
        sphereCollider = this.gameObject.AddComponent<SphereCollider>();
        sphereCollider.radius = 5;
        sphereCollider.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
        //colliders = Physics.OverlapSphere(transform.position, 5, 7);
        //for (int i = 0; i < colliders.Length; i++)
        //{
        //    if (colliders[i].gameObject.CompareTag("Player"))
        //    {
        //        playerLockOn = true;
        //        Debug.Log("true");
        //    }
        //    else
        //    {
        //        playerLockOn = false;
        //        Debug.Log("false");
        //    }
        //}


        if (playerLockOn == true && doneOnce == false)
        {
            if (startLook == true)
            {
                transform.LookAt(player.transform.position);
                startLook = false;
            }
            if (Physics.Raycast(transform.position, player.transform.position, out hit, 5))
            {
                if (hit.collider.gameObject.CompareTag("Player"))
                {
                    Debug.LogError("Ray hit");
                    animator.SetTrigger("Attack");
                    doneOnce = true;
                }
            }
        }
        if (moveRobot == true)
        {
            transform.position += transform.forward * flightSpeed * Time.deltaTime;
        }
    }

    public void EnemyMove()
    {
        Debug.Log("Move");
        moveRobot = true;
    }
    public void EnemyDie()
    {
        Debug.Log("Die");
        Destroy(this.gameObject);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(this.transform.position, 5);
        Gizmos.DrawLine(this.transform.position, player.transform.position);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            respawnScript.RespawnPlayer();
            Destroy(this.gameObject);
        }
        if (collision.gameObject.CompareTag("Bounce Pad") || collision.gameObject.CompareTag("Obstacle"))
        {
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLockOn = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            playerLockOn = false;
        }
    }
}
