using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    CharacterController controller;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] bool Grounded;
    Camera cameraMain;
    Vector3 movement;
    Vector3 direction;
    float dropSpeed = 0;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        cameraMain = Camera.main;
    }

    void HandleGravity()
    {
        if (controller.isGrounded)
            dropSpeed = -1f;

        dropSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += dropSpeed * Time.deltaTime;
    }
    // Update is called once per frame
    void Update()
    {
        movement.y = 0;

        HandleGravity();
        HandleMovement();
        movement = Quaternion.Euler(0, cameraMain.transform.eulerAngles.y, 0) * movement; //handle camera rotation

        Quaternion movementForward = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, movementForward, 100);

        controller.Move(movement);
        Grounded = controller.isGrounded;
        //transform.rotation = Quaternion.Euler(movement);
    }

    void HandleMovement()
    {
        Vector2 movementInput = PlayerInputs.MoveInput;
        Debug.Log($"X: {PlayerInputs.MoveInput.x}, Y: {PlayerInputs.MoveInput.y}");

        movementInput *= movementInput.magnitude > 0.99 ? runningSpeed : walkingSpeed;

        if (movementInput.magnitude != 0)
            direction = Quaternion.Euler(0, cameraMain.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);

        movementInput *= Time.deltaTime;

        movement.x = movementInput.x;
        movement.z = movementInput.y;
    }
}