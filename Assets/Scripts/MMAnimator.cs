using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMAnimator : MonoBehaviour
{
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }
    // Start is called before the first frame update
    void Start()
    {
        animator.Play("Spin 1");
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
