using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Enemy1Script : MonoBehaviour
{
    public GameObject player;
    [SerializeField] float rotationSpeed;
    [SerializeField] float flightSpeed;
    RaycastHit phit;
    GameObject raycastHit;
    RespawnScript respawnScript;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
    }

    // Update is called once per frame
    void Update()
    {
        Physics.Raycast(this.gameObject.transform.position, Vector3.forward, out phit, 30);
        if (phit.collider.gameObject != null)
        {
            raycastHit = phit.collider.gameObject;
        }
        else
        {
            raycastHit = null;
        }
        Debug.DrawRay(this.gameObject.transform.position, Vector3.forward, Color.red, 30);
        transform.LookAt(player.transform.position);
        Debug.Log(PlayerLockOn());
        if (PlayerLockOn() == true)
        {
            Debug.Log("BOOM");
            transform.position += transform.forward * flightSpeed * Time.deltaTime;
        }
    }

    bool PlayerLockOn()
    {
        if (raycastHit == player)
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
        }
    }
}
