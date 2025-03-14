using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Script : MonoBehaviour
{
    bool invisible;
    [SerializeField] GameObject childObj;
    [SerializeField] ParticleSystem warningParticle;

    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(InvisTime());
    }

    // Update is called once per frame
    void Update()
    {
        if(childObj)
        {
            childObj.SetActive(!invisible);
        }
        else
        {
            Destroy(this.gameObject);
        }
    }

    IEnumerator InvisTime()
    {
        invisible = true;
        yield return new WaitForSeconds(3);
        warningParticle.Play();
        yield return new WaitForSeconds(2);
        invisible = false;
        yield return new WaitForSeconds(5);
        StartCoroutine(InvisTime());
    }
}
