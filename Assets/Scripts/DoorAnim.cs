using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorAnim : MonoBehaviour
{
    public Animator doorAnimator;
    [SerializeField] Animator buttonAnimator;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void ButtonPush()
    {
        buttonAnimator.SetTrigger("Button");
    }

    public void DoorMove()
    {
        doorAnimator.SetTrigger("Door");
    }
}
