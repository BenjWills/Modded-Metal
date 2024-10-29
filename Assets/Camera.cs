using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Camera : MonoBehaviour
{
    PlayerInput playerInput;
    InputAction lookAction;
    float XRotation;
    [SerializeField] float mouseSensitivity;
    [SerializeField] Transform playerBody;

    // Start is called before the first frame update
    void Start()
    {
        playerInput = GameObject.Find("Player").GetComponent<PlayerInput>();
        lookAction = playerInput.actions.FindAction("Look");
    }

    // Update is called once per frame
    void Update()
    {
        Look();
    }

    void Look()
    {
        Vector2 mouseInput = lookAction.ReadValue<Vector2>();

        float mouseX = mouseInput.x * mouseSensitivity * Time.deltaTime;
        float mouseY = mouseInput.y * mouseSensitivity * Time.deltaTime;

        XRotation -= mouseY;
        XRotation = Mathf.Clamp(XRotation, -90, 90);

        transform.localRotation = Quaternion.Euler(XRotation, 0, 0);
        playerBody.Rotate(Vector3.up, mouseX);
    }
}
