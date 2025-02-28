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
    bool inTriggerSet;

    public string[] buff1;
    public int buffI1;
    private TextMeshProUGUI buff1Txt;
    public string[] buff2;
    public int buffI2;
    private TextMeshProUGUI buff2Txt;
    public string[] debuff;
    public int debuffI;
    private TextMeshProUGUI debuffTxt;

    public TMP_InputField inputField;

    MenusScript menuScript;

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
        menuScript = GameObject.Find("Menus").GetComponent<MenusScript>();

        slotMachineRange = this.gameObject.AddComponent<SphereCollider>();
        slotMachineRange.radius = 1.2f;
        slotMachineRange.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Buff1"))
        {
            ApplyBuff1(buff1[PlayerPrefs.GetInt("Buff1")]);
            buff1Txt.text = buff1[PlayerPrefs.GetInt("Buff1")];
        }
        else
        {
            buff1Txt.text = "None";
        }
        if (PlayerPrefs.HasKey("Buff2"))
        {
            ApplyBuff2(buff2[PlayerPrefs.GetInt("Buff2")]);
            buff2Txt.text = buff2[PlayerPrefs.GetInt("Buff2")];
        }
        else
        {
            buff2Txt.text = "None";
        }
        if (PlayerPrefs.HasKey("Debuff"))
        {
            ApplyDebuff(debuff[PlayerPrefs.GetInt("Debuff")]);
            debuffTxt.text = debuff[PlayerPrefs.GetInt("Debuff")];
        }
        else
        {
            debuffTxt.text = "None";
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (inTrigger == true && PlayerPrefs.GetInt("smCoin") >= 1)
        {
            if (interactAction.triggered)
            {
                PlayerPrefs.SetInt("smCoin", PlayerPrefs.GetInt("smCoin") - 1);
                RandomStats();
                PlayerPrefs.Save();
            }
        }
        if (inTrigger == true)
        {
            Debug.Log("works");
            menuScript.interactTxt1.enabled = true;
        }
        else
        {
            Debug.Log("works2");
            menuScript.interactTxt1.enabled = false;
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
        RemoveStats();

        buffI1 = Random.Range(0, buff1.Length);
        PlayerPrefs.SetInt("Buff1", buffI1);
        ApplyBuff1(buff1[buffI1]);
        buff1Txt.text = buff1[buffI1];

        int doSecondBuff = Random.Range(0, 2);
        if (doSecondBuff == 0)
        {
            buffI2 = Random.Range(0, buff2.Length);
            PlayerPrefs.SetInt("Buff2", buffI2);
            ApplyBuff2(buff2[buffI2]);
            buff2Txt.text = buff2[buffI2];
        }
        else
        {
            PlayerPrefs.DeleteKey("Buff2");
            ApplyBuff2("None");
            buff2Txt.text = "None";
        }

        debuffI = Random.Range(0, debuff.Length);
        PlayerPrefs.SetInt("Debuff", debuffI);
        ApplyDebuff(debuff[debuffI]);
        debuffTxt.text = debuff[debuffI];
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
        ApplyBuff1(buffName);
        ApplyBuff2(buffName);
        ApplyDebuff(buffName);
    }
    void ApplyBuff1(string buffName)
    {
        if (buffName == "Increase Speed")
        {
            movementScript.sprintSpeed += 3;
        }
        else if (buffName == "Increase Jump")
        {
            movementScript.jumpForce += 5;
        }
        else if (buffName == "Increase Crouch Speed")
        {
            movementScript.crouchSpeed += 3.5f;
        }
        else if (buffName == "Quicker Slide")
        {
            movementScript.sliderForce += 100;
        }
        else
        {
            movementScript.sprintSpeed = movementScript.startSprintSpeed;
            movementScript.jumpForce = movementScript.startJumpForce;
            movementScript.crouchSpeed = movementScript.startCrouchSpeed;
            movementScript.sliderForce = movementScript.startSliderForce;
        }
    }
    void ApplyBuff2(string buffName)
    {
        if (buffName == "Dash")
        {
            movementScript.Dash();
        }
        else if (buffName == "Place Jump Pad")
        {
            movementScript.PlaceJumpPad();
        }
    }
    void ApplyDebuff(string buffName)
    {
        if (buffName == "Decrease Speed")
        {
            movementScript.sprintSpeed -= 2;
        }
        else if (buffName == "Decrease Jump")
        {
            movementScript.jumpForce -= 3;
        }
        else if (buffName == "Decrease Crouch Speed")
        {
            movementScript.crouchSpeed -= 2;
        }
        else if (buffName == "Slower Slide")
        {
            movementScript.sliderForce -= 50;
        }
        else
        {
            movementScript.sprintSpeed = movementScript.startSprintSpeed;
            movementScript.jumpForce = movementScript.startJumpForce;
            movementScript.crouchSpeed = movementScript.startCrouchSpeed;
            movementScript.sliderForce = movementScript.startSliderForce;
        }
    }
}
