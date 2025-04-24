using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpikeObstacle : MonoBehaviour
{
    RespawnScript respawnScript;
    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip attack;
    [SerializeField] AudioClip revert;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        audioSource = GetComponent<AudioSource>();
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
            Destroy(this.gameObject);
        }
    }

    public void Attack()
    {
        audioSource.clip = attack;
        audioSource.Play();
    }
    public void Revert()
    {
        audioSource.clip = revert;
        audioSource.Play();
    }
}
