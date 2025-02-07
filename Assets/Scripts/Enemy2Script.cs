using System.Collections;
using System.Collections.Generic;
using UnityEditor.EventSystems;
using UnityEngine;

public class Enemy2Script : MonoBehaviour
{
    RespawnScript respawnScript;
    GameObject childObj;
    GameObject thisObj;
    GameObject player;
    MeshRenderer meshRenderer;
    [SerializeField] float sizeExpansion;
    SphereCollider playerArea;
    RaycastHit hit;
    bool canDie = false;
    [SerializeField] ParticleSystem chargeParticles;

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
        playerArea = this.gameObject.AddComponent<SphereCollider>();
        playerArea.radius = 5;
        playerArea.isTrigger = true;
    }

    // Update is called once per frame
    void Update()
    {
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (canDie == true)
        {
            respawnScript.RespawnPlayer();
            Debug.Log("BOOM");
            Destroy(this.gameObject);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Player"))
        {
            chargeParticles.Play();
            StartCoroutine(OnTouchTime());
        }
    }

    void OnTouch()
    {
    }

    IEnumerator OnTouchTime()
    {
        yield return new WaitForSeconds(3);
        meshRenderer.enabled = true;
        childObj.SetActive(true);
        canDie = true;
        while (thisObj.transform.localScale.x < sizeExpansion)
        {
            thisObj.transform.localScale = new Vector3(thisObj.transform.localScale.x + 0.1f, thisObj.transform.localScale.y + 0.1f, thisObj.transform.localScale.z + 0.1f);
            yield return null;
        }
        yield return new WaitForSeconds(1);
        Destroy(this.gameObject);
    }
}
