using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    [SerializeField] Transform playerBody;
    Rigidbody rb;

    PlayerInput playerInput;
    InputAction moveAction;
    [SerializeField] float moveSpeed;
    Vector2 moveInput;
    Vector3 MoveDirection;
    [SerializeField] float groundDrag;
    [SerializeField] Transform orientation;

    bool isGrounded;

    [SerializeField] float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        readyToJump = true;
        MyInput();
        SpeedControl();
        GroundCheck();
        if (isGrounded == true)
        {
            rb.drag = groundDrag;
        }
        else
        {
            rb.drag = 0;
        }
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GroundCheck()
    {
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y + 0.5f, transform.position.z), transform.lossyScale / 2, -transform.up, transform.rotation, 1) == true)
        {
            isGrounded = true;
        }
        else
        {
            isGrounded = false;
        }
    }

    void MyInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();
    }

    private void MovePlayer()
    {
        MoveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        if (isGrounded == true)
        {
            rb.AddForce(MoveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(MoveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
        }
    }

    private void SpeedControl()
    {
        Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

        if (flatVel.magnitude > moveSpeed)
        {
            Vector3 limitedVel = flatVel.normalized * moveSpeed;
            rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
        }
    }

    public void OnJump()
    {
        if (isGrounded == true && readyToJump == true) 
        {
            readyToJump = false;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    private void ResetJump()
    {
        readyToJump = true;
    }

    public void OnSlide()
    {

    }
}
