using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Script : MonoBehaviour
{
    RespawnScript respawnScript;
    bool invisable;
    [SerializeField] GameObject childObj;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        StartCoroutine(InvisTime());
    }

    // Update is called once per frame
    void Update()
    {
        if (invisable == true)
        {
            childObj.SetActive(false);
        }
        else
        {
            childObj.SetActive(true);
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
    IEnumerator InvisTime()
    {
        invisable = true;
        yield return new WaitForSeconds(5);
        invisable = false;
        yield return new WaitForSeconds(5);
        StartCoroutine(InvisTime());
    }
}
