using JetBrains.Annotations;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class Movement : MonoBehaviour
{
    [Header("Player")]
    public Transform playerBody;
    Rigidbody rb;
    [SerializeField] Camera pcam;

    [Header("Inputs")]
    PlayerInput playerInput;
    InputAction moveAction;
    InputAction sprintAction;
    InputAction crouchAction;
    InputAction slideAction;
    InputAction activateAbilityAction;

    [Header("Movement")]
    public float startSprintSpeed;
    [SerializeField] float moveSpeed;
    public float walkSpeed;
    public float sprintSpeed;
    Vector2 moveInput;
    Vector3 moveDirection;
    [SerializeField] float groundDrag;
    [SerializeField] Transform orientation;
    [SerializeField] float dashForce;

    [Header("Bools")]
    bool isGrounded;
    bool isCrouching = false;
    bool isSprinting = false;
    bool isSliding = false;

    [Header("Jump")]
    public float startJumpForce;
    public float jumpForce;
    [SerializeField] float jumpCooldown;
    [SerializeField] float startJumpCooldown;
    [SerializeField] float airMultiplier;
    bool readyToJump;

    [Header("Crouch")]
    public float startCrouchSpeed;
    public float crouchSpeed;
    [SerializeField] float crouchYScale;
    public float startXScale;
    public float startYScale;
    public float startZScale;
    RaycastHit hit;

    [Header("Slope")]
    bool exitingSlope;
    [SerializeField] float maxSlopeAngle;
    [SerializeField] float playerHeight;
    RaycastHit slopeHit;

    [Header("Sliding")]
    [SerializeField] float maxSliderTime;
    public float startSliderForce;
    public float sliderForce;
    float sliderTimer;

    [Header("Ground Check")]
    public Vector3 startgcObject;
    public Transform gcObject;
    public float startSphereSize;
    public float sphereSize;
    public LayerMask groundMask;

    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
        sprintAction = playerInput.actions.FindAction("Sprint");
        crouchAction = playerInput.actions.FindAction("Crouch");
        slideAction = playerInput.actions.FindAction("Slide");
        activateAbilityAction = playerInput.actions.FindAction("ActivateAbility");
        rb = GetComponent<Rigidbody>();

        startSphereSize = sphereSize;
        startSprintSpeed = sprintSpeed;
        startCrouchSpeed = crouchSpeed;
        startJumpForce = jumpForce;
        startSliderForce = sliderForce;
        startJumpCooldown = jumpCooldown;

        startgcObject = gcObject.position;
        startXScale = transform.localScale.x;
        startYScale = transform.localScale.y;
        startZScale = transform.localScale.z;
    }

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        readyToJump = true;
        MyInput();
        SpeedControl();
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
        isGrounded = GroundCheck();
        if (isSliding == true)
        {
            SlidingMovement();
        }
    }

    //Checks for an object beneath the player
    bool GroundCheck()
    {
        if (Physics.CheckSphere(gcObject.position, sphereSize, groundMask))
        {
            return true;
        }
        return false;
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(gcObject.position, sphereSize);
    }

    //Checks if a player is pressing a key and activates the movement
    void MyInput()
    {
        moveInput = moveAction.ReadValue<Vector2>();

        if (slideAction.triggered && (moveInput.x != 0 || moveInput.y != 0) && isSliding == false)
        {
            StartSlide();
        }
        else if (slideAction.triggered && isSliding == true)
        {
            StopSlide();
        }

        if (isSprinting == false && isCrouching == false && isSliding == false)
        {
            moveSpeed = walkSpeed;
        }

        if (crouchAction.triggered && isCrouching == false)
        {
            isSprinting = false;
            isCrouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
        }
        else if (crouchAction.triggered && isCrouching == true && !CantStand())
        {
            isCrouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }

        if (sprintAction.triggered == true && isGrounded == true && isSprinting == false)
        {
            isCrouching = false;
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
            isSprinting = true;
            moveSpeed = sprintSpeed;
        }
        else if (sprintAction.triggered == true && isSprinting == true)
        {
            isSprinting = false;
        }
    }

    //applys movement to the player using rigidbidy forces
    private void MovePlayer()
    {
        moveDirection = orientation.forward * moveInput.y + orientation.right * moveInput.x;

        if (OnSlope() && !exitingSlope)
        {
            rb.AddForce(GetSlopeMoveDirection() * moveSpeed * 20, ForceMode.Force);

            if (rb.velocity.y > 0)
            {
                rb.AddForce(Vector3.down * 80, ForceMode.Force);
            }
        }
        else if (isGrounded == true)
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10, ForceMode.Force);
        }
        else
        {
            rb.AddForce(moveDirection.normalized * moveSpeed * 10 * airMultiplier, ForceMode.Force);
        }

        rb.useGravity = !OnSlope();
    }

    //controls speed so it wont increase past a cetain point
    private void SpeedControl()
    {
        if (OnSlope() && !exitingSlope)
        {
            if (rb.velocity.magnitude > moveSpeed)
            {
                rb.velocity = rb.velocity.normalized * moveSpeed;
            }
        }
        else
        {
            Vector3 flatVel = new Vector3(rb.velocity.x, 0, rb.velocity.z);

            if (flatVel.magnitude > moveSpeed)
            {
                Vector3 limitedVel = flatVel.normalized * moveSpeed;
                rb.velocity = new Vector3(limitedVel.x, rb.velocity.y, limitedVel.z);
            }
        }
    }

    //checks if a player is on a slope
    private bool OnSlope()
    {
        if (Physics.Raycast(transform.position, Vector3.down, out slopeHit, playerHeight * 0.5f + 0.3f))
        {
            float angle = Vector3.Angle(Vector3.up, slopeHit.normal);
            return angle < maxSlopeAngle && angle != 0;
        }
        else
        {
            return false;
        }
    }

    //changes player movement based on slope angle
    private Vector3 GetSlopeMoveDirection()
    {
        return Vector3.ProjectOnPlane(moveDirection, slopeHit.normal).normalized;
    }

    public bool CantStand()
    {
        if (Physics.Raycast(transform.position, Vector3.up, out hit, playerHeight * 0.5f + 0.3f))
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    //applies a rigidbidy force to make the player jump
    public void OnJump()
    {
        if (isGrounded == true && readyToJump == true) 
        {
            exitingSlope = true;
            readyToJump = false;

            rb.velocity = new Vector3(rb.velocity.x, 0, rb.velocity.z);
            rb.AddForce(transform.up * jumpForce, ForceMode.Impulse);

            Invoke(nameof(ResetJump),jumpCooldown);
        }
    }

    //resets the player jump
    private void ResetJump()
    {
        exitingSlope = false;
        readyToJump = true;
    }

    //starts the players slide
    void StartSlide()
    {
        isSliding = true;
        isCrouching = false;

        transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
        rb.AddForce(Vector3.down * 5, ForceMode.Impulse);

        sliderTimer = maxSliderTime;
    }

    //applies a rigidbody force to make the player slide
    void SlidingMovement()
    {
        rb.AddForce(moveDirection.normalized * sliderForce, ForceMode.Force);

        sliderTimer -= Time.deltaTime;

        if (sliderTimer <= 0)
        {
            StopSlide();
        }
    }

    //stops the player slide
    void StopSlide()
    {
        isSliding = false;
        if (CantStand())
        {
            isCrouching = true;
            transform.localScale = new Vector3(transform.localScale.x, crouchYScale, transform.localScale.z);
            rb.AddForce(Vector3.down * 5, ForceMode.Impulse);
            moveSpeed = crouchSpeed;
        }
        else
        {
            transform.localScale = new Vector3(transform.localScale.x, startYScale, transform.localScale.z);
        }
    }

    public void Dash()
    {
        if (!CantStand())
        {
            if (activateAbilityAction.triggered)
            {
                rb.AddForce(pcam.transform.forward * dashForce, ForceMode.Impulse);
            }
        }
    }

    public void PlaceJumpPad()
    {
        if (activateAbilityAction.triggered)
        {
            Debug.Log("Placed jump pad!");
        }
    }
}
