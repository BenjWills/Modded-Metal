using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SlotMachine : MonoBehaviour
{
    Movement movementScript;
    PlayerInput playerInput;
    InputAction interactAction;

    Transform playerPos;
    Transform slotMachine;

    SphereCollider slotMachineRange;
    bool inTrigger;

    public string[] buff1;
    int buffI1;
    private TextMeshProUGUI buff1Txt;
    public string[] buff2;
    int buffI2;
    private TextMeshProUGUI buff2Txt;
    public string[] debuff;
    int debuffI;
    private TextMeshProUGUI debuffTxt;

    private void Awake()
    {
        slotMachine = GameObject.Find("Slot Machine").GetComponent<Transform>();
        playerPos = GameObject.Find("PlayerOBJ").GetComponent<Transform>();
        movementScript = GameObject.Find("Player").GetComponent<Movement>();
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        buff1Txt = GameObject.Find("Buff1Text (TMP)").GetComponent<TextMeshProUGUI>();
        buff2Txt = GameObject.Find("Buff2Text (TMP)").GetComponent<TextMeshProUGUI>();
        debuffTxt = GameObject.Find("DebuffText (TMP)").GetComponent<TextMeshProUGUI>();

        slotMachineRange = this.gameObject.AddComponent<SphereCollider>();
        slotMachineRange.radius = 1.2f;
        slotMachineRange.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        buff1Txt.text = "None";
        buff2Txt.text = "None";
        debuffTxt.text = "None";
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger == true)
        {
            if (interactAction.triggered)
            {
                RandomStats();
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            inTrigger = false;
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
        buff1Txt.text = buff1[buffI1];
        ApplyStats(buff1[buffI1]);

        if (doSecondBuff == 0)
        {
            buff2Txt.text = buff2[buffI2];
            ApplyStats(buff2[buffI2]);
        }
        else
        {
            buff2Txt.text = "None";
        }
        debuffTxt.text = debuff[debuffI];
        ApplyStats(debuff[debuffI]);
    }
    public void RemoveStats()
    {
        buff1Txt.text = "None";
        buff2Txt.text = "None";
        debuffTxt.text = "None";

        movementScript.moveSpeed = 5;
        movementScript.jumpForce = 8;
        movementScript.sliderForce = 200;
        movementScript.playerBody.localScale = new Vector3(transform.localScale.x, movementScript.startYScale, transform.localScale.z);
    }

    void ApplyStats(string buffName)
    {
        if (buffName == "Increase Speed")
        {
            movementScript.moveSpeed += 5;
        }
        else
        {
            movementScript.moveSpeed = 5;
        }
        if (buffName == "Increase Jump")
        {
            movementScript.jumpForce += 5;
        }
        else
        {
            movementScript.jumpForce = 8;
        }
        if (buffName == "Quicker Slide")
        {
            movementScript.sliderForce += 100;
        }
        else
        {
            movementScript.sliderForce = 200;
        }
        if (buffName == "Decrease Size")
        {
            movementScript.playerBody.localScale = new Vector3(movementScript.playerBody.localScale.x, movementScript.playerBody.localScale.y / 2, movementScript.playerBody.localScale.z);
        }
        else
        {
            movementScript.playerBody.localScale = new Vector3(transform.localScale.x, movementScript.startYScale, transform.localScale.z);
        }
        if (buffName == "Increase Crouch Speed")
        {
            movementScript.crouchSpeed += 3.5f;
        }
        else
        {
            movementScript.crouchSpeed = 3.5f;
        }
        if (buffName == "Increase Size")
        {
            movementScript.playerBody.localScale = new Vector3(movementScript.playerBody.localScale.x, movementScript.playerBody.localScale.y * 1.8f, movementScript.playerBody.localScale.z);
        }
        else
        {
            movementScript.playerBody.localScale = new Vector3(transform.localScale.x, movementScript.startYScale, transform.localScale.z);
        }
        if (buffName == "Reduce Speed")
        {
            movementScript.moveSpeed -= 3;
        }
        else
        {
            movementScript.moveSpeed = 5;
        }
        if (buffName == "Reduce Jump")
        {
            movementScript.jumpForce -= 3;
        }
        else
        {
            movementScript.jumpForce = 8;
        }
    }
}
