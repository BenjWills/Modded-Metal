using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class SlotMachine : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction interactAction;

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
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        interactAction = playerInput.actions.FindAction("Interact");
        buff1Txt = GameObject.Find("Buff1Text (TMP)").GetComponent<TextMeshProUGUI>();
        buff2Txt = GameObject.Find("Buff2Text (TMP)").GetComponent<TextMeshProUGUI>();
        debuffTxt = GameObject.Find("DebuffText (TMP)").GetComponent<TextMeshProUGUI>();
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
        if (Physics.SphereCast(new Vector3(0, 0, 0), 3, transform.forward, out RaycastHit hit) == true)
        {
            if (hit.collider.gameObject.CompareTag("Player") && interactAction.triggered)
            {
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
        buff1Txt.text = buff1[buffI1];

        if (doSecondBuff == 0)
        {
            buff2Txt.text = buff2[buffI2];
        }
        else
        {
            buff2Txt.text = "None";
        }
        debuffTxt.text = debuff[debuffI];
    }
}
