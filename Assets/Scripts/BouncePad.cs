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
    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        settingsScript = GameObject.FindGameObjectWithTag("Settings").GetComponent<Settings>();
        orientation = GameObject.Find("Orientation").GetComponent<Transform>();
        audioSource = GetComponent<AudioSource>();
        rb = this.gameObject.GetComponent<Rigidbody>();
        rb.AddForce(orientation.forward * 10, ForceMode.Impulse);
        StartCoroutine(BouncePadEffectBuffer());
        if (this.gameObject.CompareTag("Bounce Pad"))
        {
            settingsScript.bouncePads.Add(this.gameObject);
        }
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
    public void DestroyBouncePad()
    {
        for (int i = 0; i < settingsScript.bouncePads.Count; i++)
        {
            Destroy(settingsScript.bouncePads[i]);
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player") && canBeUsed == true)
        {
            audioSource.Play();
            collision.gameObject.GetComponent<Rigidbody>().AddForce(forceApplied, ForceMode.Impulse);
        }
    }

    IEnumerator BouncePadEffectBuffer()
    {
        yield return new WaitForSeconds(0.3f);
        canBeUsed = true;
    }
}
