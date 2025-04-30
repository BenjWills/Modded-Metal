using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MMAnimator : MonoBehaviour
{
    Animator animator;
    public SpriteRenderer spriteRenderer1;
    public SpriteRenderer spriteRenderer2;
    public SpriteRenderer spriteRenderer3;
    public Sprite[] buff1;
    public Sprite[] buff2;
    public Sprite[] debuff;
    public GameObject go1;
    public GameObject go2;
    public GameObject go3;

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
    public void HideImage()
    {
        go1.SetActive(false);
        go2.SetActive(false);
        go3.SetActive(false);
    }
    public void RandomImage()
    {
        go1.SetActive(true);
        go2.SetActive(true);
        go3.SetActive(true);
        spriteRenderer1.sprite = buff1[Random.Range(0, buff1.Length)];
        spriteRenderer2.sprite = buff2[Random.Range(0, buff2.Length)];
        spriteRenderer3.sprite = debuff[Random.Range(0, debuff.Length)];
    }
}
