using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Movement : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction moveAction;
    [SerializeField] float moveSpeed;

    bool isGrounded;
    Rigidbody rb;
    [SerializeField] float jumpForce;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        moveAction = playerInput.actions.FindAction("Move");
    }

    // Update is called once per frame
    void Update()
    {
        MovePlayer();
    }

    void GroundCheck()
    {
        if (Physics.BoxCast(new Vector3(transform.position.x, transform.position.y -1, transform.position.z), transform.lossyScale / 2, -transform.up, transform.rotation, 1) == true)
        {
            Debug.Log("Grounded");
            isGrounded = true;
        }
    }

    private void MovePlayer()
    {
        Vector2 direction = moveAction.ReadValue<Vector2>();
        transform.position += new Vector3(direction.x, 0, direction.y) * moveSpeed * Time.deltaTime;
    }

    public void Jump(InputAction.CallbackContext context)
    {
        if (isGrounded == true) 
        {
            rb.AddForce(new Vector3(transform.position.x, jumpForce, transform.position.z), ForceMode.Force);
        }
    }
}
