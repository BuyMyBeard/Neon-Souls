using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [Range(0, 1)]
    [SerializeField] float runThreshold = .7f;
    [Range(0, 1)]
    [SerializeField] float deadZone = .1f;
    [SerializeField] float turnSpeed = 100;
    new Camera camera;
    Vector3 movement;
    Vector3 direction = Vector3.forward;
    PlayerController playerController;
    Animator animator;

    [HideInInspector]
    public bool frozen = false;
    public Vector2 MovementDirection { get; private set; } = Vector2.zero;
    float dropSpeed = 0;
    void Awake()
    {
        characterController = GetComponentInChildren<CharacterController>();
        camera = Camera.main;
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
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

        if (frozen)
            return;

        HandleGravity();
        HandleMovement();

        movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * movement; //handle camera rotation

        Quaternion movementForward = Quaternion.LookRotation(direction, Vector3.up);
        characterController.transform.rotation = Quaternion.RotateTowards(characterController.transform.rotation, movementForward, turnSpeed * Time.deltaTime);

        characterController.Move(movement);
    }

    void HandleMovement()
    {
        animator.ResetTrigger("QuickWalk");
        animator.ResetTrigger("Walk");
        animator.ResetTrigger("Idle");
        

        Vector2 movementInput = playerController.Move; 
        float movementMagnitude = movementInput.magnitude;
        
        if (movementMagnitude >= deadZone) 
        {
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);

            if (movementMagnitude >= runThreshold)
            {
                movementInput = runningSpeed * movementInput.normalized;
                animator.SetTrigger("QuickWalk");
            }
            else
            {
                movementInput = walkingSpeed * movementInput.normalized;
                animator.SetTrigger("Walk");
            }
        }
        else
        {
            movementInput = Vector2.zero;
            animator.SetTrigger("Idle");
        }
        animator.SetFloat("Speed", movementInput.magnitude);

        movementInput *= Time.deltaTime;
        movement.x = movementInput.x;
        movement.z = movementInput.y;
        MovementDirection = movementInput * direction;
    }
    
}