using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class SlotMachine : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction interactAction;

    public string[] buff1;
    int buffI1;
    public string[] buff2;
    int buffI2;
    public string[] debuff;
    int debuffI;

    private void Awake()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Physics.SphereCast(new Vector3(0, 0, 0), 3, transform.forward, out RaycastHit hit) == true)
        {
            if (hit.collider.gameObject.CompareTag("Player") && interactAction.triggered)
            {
                Debug.Log("hit");
                RandomStats();
            }
        }
    }

    void RandomStats()
    {
        buffI1 = Random.Range(0, buff1.Length);
        int doSecondBuff = Random.Range(0, 3);
        if (doSecondBuff == 0)
        {
            buffI2 = Random.Range(0, buff2.Length);
        }
        debuffI = Random.Range(0, debuff.Length);

        Debug.Log(buff1[buffI1]);
        if (doSecondBuff == 0)
        {
            Debug.Log(buff2[buffI2]);
        }
        Debug.Log(debuff[debuffI]);
    }
}
