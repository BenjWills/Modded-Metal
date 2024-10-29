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
    Transform orientation;

    bool isGrounded;
    [SerializeField] float jumpForce;
    float trueJumpForce;

    [SerializeField] float slideForce;
    float trueSlideForce;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        rb = GetComponent<Rigidbody>();
        trueJumpForce = jumpForce * 50;
        trueSlideForce = slideForce * 50;
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        GroundCheck();
    }
    private void FixedUpdate()
    {
        MovePlayer();
    }

    void GroundCheck()
    {
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y -0.2f, transform.position.z), transform.lossyScale / 2, -transform.up, transform.rotation, 1) == true)
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
        MoveDirection = orientation.forward * moveInput.x + orientation.right * moveInput.y;

        rb.AddForce(MoveDirection.normalized * moveSpeed * 10, ForceMode.Force);
    }

    public void OnJump()
    {
        if (isGrounded == true) 
        {
            rb.AddForce(new Vector3(transform.position.x, trueJumpForce, transform.position.z), ForceMode.Force);
            Debug.Log("Jump");
        }
    }

    public void OnSlide()
    {
        if (isGrounded == true)
        {

        }
    }
}
