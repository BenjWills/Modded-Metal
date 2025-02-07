using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy3Script : MonoBehaviour
{
    bool invisable;
    [SerializeField] GameObject childObj;

    // Start is called before the first frame update
    void Start()
    {
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

    IEnumerator InvisTime()
    {
        invisable = true;
        yield return new WaitForSeconds(5);
        invisable = false;
        yield return new WaitForSeconds(5);
        StartCoroutine(InvisTime());
    }
}
