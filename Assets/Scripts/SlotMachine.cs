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

    public Vector3 tallPos;
    public Vector3 shortPos;

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
        buff1Txt = GameObject.Find("Buff1 Text (TMP)").GetComponent<TextMeshProUGUI>();
        buff2Txt = GameObject.Find("Buff2 Text (TMP)").GetComponent<TextMeshProUGUI>();
        debuffTxt = GameObject.Find("Debuff Text (TMP)").GetComponent<TextMeshProUGUI>();

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
        ApplyStats("None");
        buff1Txt.text = "None";
        buff2Txt.text = "None";
        debuffTxt.text = "None";
    }

    void ApplyStats(string buffName)
    {
        movementScript.moveSpeed = ApplySpeed(buffName);
        movementScript.jumpForce = ApplyJump(buffName);
        movementScript.crouchSpeed = ApplyCrouch(buffName);
        movementScript.sliderForce = ApplySlide(buffName);
        movementScript.playerBody.localScale = ApplySize(buffName);

        if (movementScript.playerBody.localScale.y < movementScript.startYScale)
        {
            movementScript.sphereSize = movementScript.sphereSize * 1.8f;
            movementScript.jumpForce += 5;
            movementScript.gcObject.position = shortPos;
        }
        else if (movementScript.playerBody.localScale.y > movementScript.startYScale)
        {
            movementScript.sphereSize = movementScript.sphereSize / 2;
            movementScript.jumpForce = movementScript.startJumpForce;
            movementScript.gcObject.position = tallPos;
        }
        else
        {
            Debug.Log("burh");
            movementScript.sphereSize = movementScript.startSphereSize;
            movementScript.gcObject.position = new Vector3(0, 0.6f, 0);
        }
    }
    float ApplySpeed(string speed)
    {
        switch (speed)
        {
            default:
                return movementScript.moveSpeed = movementScript.startMoveSpeed;
            case "Increase Speed":
                return movementScript.moveSpeed += 5;
            case "Decrease Speed":
                return movementScript.moveSpeed -= 3;
        }
    }
    float ApplyJump(string jump)
    {
        switch (jump)
        {
            default:
                return movementScript.jumpForce = movementScript.startJumpForce;
            case "Increase Jump":
                return movementScript.jumpForce += 5;
            case "Decrease Jump":
                return movementScript.jumpForce -= 3;
        }
    }
    float ApplyCrouch(string crouch)
    {
        switch (crouch)
        {
            default:
                return movementScript.crouchSpeed = movementScript.startCrouchSpeed;
            case "Increase Crouch Speed":
                return movementScript.crouchSpeed += 3.5f;
            case "Decrease Crouch Speed":
                return movementScript.crouchSpeed -= 2;
        }
    }
    float ApplySlide(string slide)
    {
        switch (slide)
        {
            default:
                return movementScript.sliderForce = movementScript.startSliderForce;
            case "Quicker Slide":
                return movementScript.sliderForce += 100;
            case "Slower Slide":
                return movementScript.sliderForce -= 50;
        } 
    }
    Vector3 ApplySize(string size)
    {
        switch (size)
        {
            default:
                return movementScript.playerBody.localScale = new Vector3(movementScript.startXScale, movementScript.startYScale, movementScript.startZScale);
            case "Decrease Size":
                return movementScript.playerBody.localScale = new Vector3(movementScript.playerBody.localScale.x, movementScript.playerBody.localScale.y / 2, movementScript.playerBody.localScale.z);
            case "Increase Size":
                return movementScript.playerBody.localScale = new Vector3(movementScript.playerBody.localScale.x, movementScript.playerBody.localScale.y * 1.8f, movementScript.playerBody.localScale.z);
        }
    }
}
