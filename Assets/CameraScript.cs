using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CameraScript : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction lookAction;
    float XRotation;
    float YRotation;
    [SerializeField] float mouseSensX;
    [SerializeField] float mouseSensY;
    [SerializeField] Transform playerBody;
    [SerializeField] Transform orientation;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.Find("PlayerOBJ").GetComponent<PlayerInput>();
        lookAction = playerInput.actions.FindAction("Look");
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    void Look()
    {
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();

        float mouseX = mouseInput.x * mouseSensX * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensY * Time.deltaTime;

        YRotation += mouseX;
        XRotation -= mouseY;

        XRotation = Mathf.Clamp(XRotation, -90, 90);

        transform.rotation = Quaternion.Euler(XRotation, YRotation, 0);
        orientation.rotation = Quaternion.Euler(0, YRotation, 0);
    }
}
