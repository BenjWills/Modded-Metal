using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    public Animator doorAnimator;
    [SerializeField] Animator buttonAnimator;
    SpawnLevel spawnLevel;

    // Start is called before the first frame update
    void Start()
    {
        spawnLevel = GameObject.Find("SpawnLevel").GetComponent<SpawnLevel>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPush()
    {
        buttonAnimator.SetTrigger("Button");
    }

    public void DoorOpen()
    {
        doorAnimator.SetBool("Door", true);
    }
    public void DoorClose()
    {
        doorAnimator.SetBool("Door", false);
    }
    public void DespawnLevel()
    {
        spawnLevel.DespawnLevel();
    }
}
