using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy1Script : MonoBehaviour
{
    [SerializeField] GameObject player;
    [SerializeField] float flightSpeed;
    RaycastHit phit;
    GameObject raycastHit;
    RespawnScript respawnScript;
    bool timeDone;
    bool waitDone = false;
    [SerializeField] int destructionTimeInt;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerLockOn() == true)
        {
            transform.position += transform.forward * flightSpeed * Time.deltaTime;
            StartCoroutine(DestructionTimer());
        }
        else
        {
            StartCoroutine(Wait());
            if (waitDone == true)
            {
                transform.LookAt(player.transform.position);
                transform.rotation = new Quaternion(0, transform.rotation.y, 0, transform.rotation.w);
            }
        }

        if (Physics.Raycast(this.gameObject.transform.position, transform.forward, out phit, 10))
        {
            raycastHit = phit.collider.gameObject;
        }
        else
        {
            raycastHit = null;
        }
    }

    bool PlayerLockOn()
    {
        if (raycastHit != null)
        {
            if (raycastHit.CompareTag("Player"))
            {
                return true;
            }
            else
            {
                return false;
            }
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

    IEnumerator Wait()
    {
        yield return new WaitForSeconds(1);
        waitDone = true;
    }
}
