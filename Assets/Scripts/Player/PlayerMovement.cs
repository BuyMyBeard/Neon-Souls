using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float runTreshold = .7f;
    [SerializeField] float deadZone = .1f;
    [SerializeField] bool Grounded;
    [SerializeField] float turnSpeed = 100;
    new Camera camera;
    Vector3 movement;
    Vector3 direction = Vector3.forward;
    PlayerController playerController;

    public Vector2 MovementDirection { get; private set; } = Vector2.zero;
    float dropSpeed = 0;
    void Awake()
    {
        characterController = GetComponentInChildren<CharacterController>();
        camera = Camera.main;
        playerController = GetComponent<PlayerController>();
    }

    void HandleGravity()
    {
        if (characterController.isGrounded)
            dropSpeed = -1f;

        dropSpeed += Physics.gravity.y * Time.deltaTime;
        movement.y += dropSpeed * Time.deltaTime;
    }
    void Update()
    {
        movement.y = 0;

        HandleGravity();
        HandleMovement();
        movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * movement; //handle camera rotation

        Quaternion movementForward = Quaternion.LookRotation(direction, Vector3.up);
        characterController.transform.rotation = Quaternion.RotateTowards(characterController.transform.rotation, movementForward, turnSpeed * Time.deltaTime);

        characterController.Move(movement);
        Grounded = characterController.isGrounded;
    }

    void HandleMovement()
    {
        Vector2 movementInput = playerController.MoveInput; 
        float movementMagnitude = movementInput.magnitude;

        if (movementMagnitude >= deadZone)
        {
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);

            if (movementMagnitude >= runTreshold)
            {
                movementInput = runningSpeed * movementInput.normalized;
                // TODO: set animation state
            }
            else
            {
                movementInput = walkingSpeed * movementInput.normalized;
                // TODO: set animation state
            }
        }
        else
        {
            movementInput *= 0;
        }

        movementInput *= Time.deltaTime;
        movement.x = movementInput.x;
        movement.z = movementInput.y;
        MovementDirection = movementInput * direction;
    }
}