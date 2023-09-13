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
        Vector2 joystickV = PlayerInputs.MoveControllerDirection;
        HandleGravity();


        if (joystickV.magnitude > 0)
        {
            HandleJoystickMovement(joystickV);
        }
        else
        {
            HandleKeyboardMovement();
        }



        movement = Quaternion.Euler(0, cameraMain.transform.eulerAngles.y, 0) * movement; //handle camera rotation

        Quaternion movementForward = Quaternion.LookRotation(direction, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, movementForward, 100);

        controller.Move(movement);
        Grounded = controller.isGrounded;
        //transform.rotation = Quaternion.Euler(movement);
    }

    void HandleJoystickMovement(Vector3 joystickV)
    {
        Debug.Log($"X: {PlayerInputs.MoveControllerDirection.x}, Y: {PlayerInputs.MoveControllerDirection.y}");


        joystickV *= joystickV.magnitude > 0.99 ? runningSpeed : walkingSpeed;

        if (joystickV.magnitude != 0)
            direction = Quaternion.Euler(0, cameraMain.transform.eulerAngles.y, 0) * new Vector3(joystickV.x, 0, joystickV.y);

        joystickV *= Time.deltaTime;

        movement.x = joystickV.x;
        movement.z = joystickV.y;

    }
    void HandleKeyboardMovement()
    {
        Vector2 keyboardV = PlayerInputs.MoveKeyboardDirection;
        keyboardV *= PlayerInputs.IsRunning ? runningSpeed : walkingSpeed;
        
        if(keyboardV.magnitude != 0)
            direction = Quaternion.Euler(0, cameraMain.transform.eulerAngles.y, 0) * new Vector3(keyboardV.x, 0, keyboardV.y);
        
        keyboardV *= Time.deltaTime;
        
        movement.x = keyboardV.x;
        movement.z = keyboardV.y;
    }
}