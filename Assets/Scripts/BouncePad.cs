using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BouncePad : MonoBehaviour
{
    [SerializeField] Vector3 forceApplied;
    Rigidbody rb;
    bool canBeUsed = false;
    Settings settingsScript;
    Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
        settingsScript = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        orientation = GameObject.Find("Orientation").GetComponent<Transform>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * 10, ForceMode.Impulse);
        StartCoroutine(BouncePadEffectBuffer());
        settingsScript.bouncePads.Add(this.gameObject);
        if (settingsScript.bouncePads.Count > 3)
        {
            Destroy(settingsScript.bouncePads[0]);
            settingsScript.bouncePads.Remove(settingsScript.bouncePads[0]);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canBeUsed == true)
        {
            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceApplied, ForceMode.Impulse);
        }
    }

    IEnumerator BouncePadEffectBuffer()
    {
        yield return new WaitForSeconds(0.3f);
        canBeUsed = true;
    }
}
