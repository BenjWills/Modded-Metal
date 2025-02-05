using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RespawnScript : MonoBehaviour
{
    [SerializeField] float lowestLevel;
    GameObject player;
    float playerHeight;
    public Transform respawnPoint;
    [SerializeField] Camera pCam;

    [SerializeField] ParticleSystem explosion;

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
            PlayerPrefs.SetInt("deathTotal", PlayerPrefs.GetInt("deathTotal") + 1);
            player.transform.position = respawnPoint.position;
            PlayerPrefs.Save();
        }
    }

    public void RespawnPlayer()
    {
        PlayerPrefs.SetInt("deathTotal", PlayerPrefs.GetInt("deathTotal") + 1);
        StartCoroutine(ExplosionTime());
        PlayerPrefs.Save();
    }

    IEnumerator ExplosionTime()
    {
        pCam.transform.position = new Vector3(3, 0, 0);
        yield return new WaitForSeconds(0.1f);
        explosion.Play();
        yield return new WaitForSeconds(1);
        pCam.transform.position = new Vector3(0, 0, 0);
        player.transform.position = respawnPoint.position;
    }
}
