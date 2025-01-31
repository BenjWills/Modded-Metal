using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy2Script : MonoBehaviour
{
    RespawnScript respawnScript;
    GameObject childObj;
    GameObject thisObj;
    GameObject player;
    MeshRenderer meshRenderer;
    [SerializeField] Vector3 sizeExpansion;
    RaycastHit hit;
    bool canDie;

    // Start is called before the first frame update
    void Start()
    {
        respawnScript = GameObject.Find("Respawn Point").GetComponent<RespawnScript>();
        childObj = GameObject.Find("o2Child");
        thisObj = this.gameObject;
        player = GameObject.FindGameObjectWithTag("Player");
        meshRenderer = this.gameObject.GetComponent<MeshRenderer>();
        childObj.SetActive(false);
        meshRenderer.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.Raycast(this.gameObject.transform.position, player.transform.position, out hit, 3))
        {
            StartCoroutine(OnTouchTime());
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (canDie == true)
        {
            respawnScript.RespawnPlayer();
            this.gameObject.SetActive(false);
        }
    }

    void OnTouch()
    {
        meshRenderer.enabled = true;
        childObj.SetActive(true);
        if (thisObj.transform.localScale != sizeExpansion)
        {
            thisObj.transform.localScale = new Vector3(thisObj.transform.localScale.x + 0.1f, thisObj.transform.localScale.y + 0.1f, thisObj.transform.localScale.z + 0.1f);
        }
        else
        {
            StartCoroutine(DestroyTime());
        }
    }

    IEnumerator OnTouchTime()
    {
        yield return new WaitForSeconds(1);
        OnTouch();
    }
    IEnumerator DestroyTime()
    {
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
