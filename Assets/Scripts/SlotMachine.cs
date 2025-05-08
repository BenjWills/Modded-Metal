using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;
using UnityEngine.UI;

public class SlotMachine : MonoBehaviour
{
    Movement movementScript;
    PlayerInput playerInput;
    InputAction interactAction;

    Transform playerPos;
    Transform slotMachine;

    SphereCollider slotMachineRange;
    bool inTrigger;
    bool dashBought;
    bool jumpPadBought;

    public string[] buff1;
    public Sprite[] buff1ImageArray;
    private Image buff1Image;
    public int buffI1;
    private TextMeshProUGUI buff1Txt;
    public string[] buff2;
    public Sprite[] buff2ImageArray;
    private Image buff2Image;
    public int buffI2;
    private TextMeshProUGUI buff2Txt;
    public string[] debuff;
    public Sprite[] debuffImageArray;
    private Image debuffImage;
    public int debuffI;
    private TextMeshProUGUI debuffTxt;
    public Sprite noneSprite;

    public GameObject plane1;
    public SpriteRenderer mat1;
    public GameObject plane2;
    public SpriteRenderer mat2;
    public GameObject plane3;
    public SpriteRenderer mat3;

    public TMP_InputField inputField;

    MenusScript menuScript;

    [SerializeField] Animator slotMachineSpin;
    AudioSource audioSource;

    private void Awake()
    {
        slotMachine = GameObject.Find("Slot Machine").GetComponent<Transform>();
        playerPos = GameObject.Find("PlayerOBJ").GetComponent<Transform>();
        movementScript = GameObject.Find("Player").GetComponent<Movement>();
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        buff1Txt = GameObject.Find("Buff1 Text (TMP)").GetComponent<TextMeshProUGUI>();
        buff1Image = GameObject.Find("Buff1 Image").GetComponent<Image>();
        buff2Txt = GameObject.Find("Buff2 Text (TMP)").GetComponent<TextMeshProUGUI>();
        buff2Image = GameObject.Find("Buff2 Image").GetComponent<Image>();
        debuffTxt = GameObject.Find("Debuff Text (TMP)").GetComponent<TextMeshProUGUI>();
        debuffImage = GameObject.Find("Debuff Image").GetComponent<Image>();
        menuScript = GameObject.Find("Menus").GetComponent<MenusScript>();
        audioSource = GetComponent<AudioSource>();

        slotMachineRange = this.gameObject.AddComponent<SphereCollider>();
        slotMachineRange.radius = 3;
        slotMachineRange.isTrigger = true;
    }

    // Start is called before the first frame update
    void Start()
    {
        if (PlayerPrefs.HasKey("Buff1"))
        {
            ApplyBuff1(buff1[PlayerPrefs.GetInt("Buff1")]);
            buff1Txt.text = buff1[PlayerPrefs.GetInt("Buff1")];
            buff1Image.sprite = buff1ImageArray[PlayerPrefs.GetInt("Buff1")];
            mat1.sprite = buff1ImageArray[PlayerPrefs.GetInt("Buff1")];
        }
        else
        {
            buff1Txt.text = "None";
            buff1Image.sprite = noneSprite;
            mat1.sprite = noneSprite;
        }
        if (PlayerPrefs.HasKey("Buff2"))
        {
            ApplyBuff2(buff2[PlayerPrefs.GetInt("Buff2")]);
            buff2Txt.text = buff2[PlayerPrefs.GetInt("Buff2")];
            buff2Image.sprite = buff2ImageArray[PlayerPrefs.GetInt("Buff2")];
            mat2.sprite = buff2ImageArray[PlayerPrefs.GetInt("Buff2")];
        }
        else
        {
            buff2Txt.text = "None";
            buff2Image.sprite = noneSprite;
            mat2.sprite = noneSprite;
        }
        if (PlayerPrefs.HasKey("Debuff"))
        {
            ApplyDebuff(debuff[PlayerPrefs.GetInt("Debuff")]);
            debuffTxt.text = debuff[PlayerPrefs.GetInt("Debuff")];
            debuffImage.sprite = debuffImageArray[PlayerPrefs.GetInt("Debuff")];
            mat3.sprite = debuffImageArray[PlayerPrefs.GetInt("Debuff")];
        }
        else
        {
            debuffTxt.text = "None";
            debuffImage.sprite = noneSprite;
            mat3.sprite = noneSprite;
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
                plane1.SetActive(false);
                plane2.SetActive(false);
                plane3.SetActive(false);
                slotMachineSpin.SetTrigger("Spin");
                audioSource.Play();
                PlayerPrefs.Save();
                slotMachineRange.radius = 0;
            }
        }
        if (inTrigger == true)
        {
            menuScript.interactTxt1.enabled = true;
        }
        else
        {
            menuScript.interactTxt1.enabled = false;
        }
        if (dashBought == true)
        {
            movementScript.Dash();
        }
        if (jumpPadBought == true)
        {
            movementScript.PlaceJumpPad();
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

    void OnGamble()
    {
        RemoveStats();

        buffI1 = Random.Range(0, buff1.Length);
        PlayerPrefs.SetInt("Buff1", buffI1);
        ApplyBuff1(buff1[buffI1]);
        buff1Txt.text = buff1[buffI1];
        buff1Image.sprite = buff1ImageArray[PlayerPrefs.GetInt("Buff1")];
        plane1.SetActive(true);
        mat1.sprite = buff1ImageArray[PlayerPrefs.GetInt("Buff1")];

        int doSecondBuff = Random.Range(0, 2);
        if (doSecondBuff == 0)
        {
            buffI2 = Random.Range(0, buff2.Length);
            PlayerPrefs.SetInt("Buff2", buffI2);
            ApplyBuff2(buff2[buffI2]);
            buff2Txt.text = buff2[buffI2];
            buff2Image.sprite = buff2ImageArray[PlayerPrefs.GetInt("Buff2")];
            plane2.SetActive(true);
            mat2.sprite = buff2ImageArray[PlayerPrefs.GetInt("Buff2")];
        }
        else
        {
            PlayerPrefs.DeleteKey("Buff2");
            ApplyBuff2("None");
            buff2Txt.text = "None";
            buff2Image.sprite = noneSprite;
            plane2.SetActive(true);
            mat2.sprite = noneSprite;
        }

        debuffI = Random.Range(0, debuff.Length);
        PlayerPrefs.SetInt("Debuff", debuffI);
        ApplyDebuff(debuff[debuffI]);
        debuffTxt.text = debuff[debuffI];
        debuffImage.sprite = debuffImageArray[PlayerPrefs.GetInt("Debuff")];
        plane3.SetActive(true);
        mat3.sprite = debuffImageArray[PlayerPrefs.GetInt("Debuff")];

        slotMachineRange.radius = 1.2f;
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
            movementScript.jumpForce += 2;
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
            if (movementScript.isSprinting == true)
            {
                movementScript.moveSpeed = movementScript.sprintSpeed;
            }
            else
            {
                movementScript.moveSpeed = movementScript.walkSpeed;
            }
        }
    }
    void ApplyBuff2(string buffName)
    {
        if (buffName == "Dash")
        {
            dashBought = true;
        }
        else if (buffName == "Place Jump Pad")
        {
            jumpPadBought = true;
        }
        else
        {
            dashBought = false;
            jumpPadBought = false;
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
            movementScript.jumpForce -= 1;
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
            if (movementScript.isSprinting == true)
            {
                movementScript.moveSpeed = movementScript.sprintSpeed;
            }
            else
            {
                movementScript.moveSpeed = movementScript.walkSpeed;
            }
        }
    }
}
