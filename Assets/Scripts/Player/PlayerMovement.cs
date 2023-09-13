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
    Camera camera;
    Vector3 movement;
    float dropSpeed = 0;
    void Awake()
    {
        controller = GetComponent<CharacterController>();
        camera = Camera.main;
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
        HandleJoystickMovement();
        //handleClavierMouvement;

        movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * movement; //handle camera rotation
        controller.Move(movement);
    }

    void HandleJoystickMovement()
    {
        Debug.Log($"X: {PlayerInputs.MoveDirection.x}, Y: {PlayerInputs.MoveDirection.y}");

        Vector2 joystickV = PlayerInputs.MoveDirection;

        joystickV *= joystickV.magnitude > 0.95 ? runningSpeed : walkingSpeed;

        movement.x = joystickV.x * Time.deltaTime;
        movement.z = joystickV.y * Time.deltaTime;
    }

}