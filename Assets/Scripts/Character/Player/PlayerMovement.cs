using UnityEngine;
[RequireComponent(typeof(LockOn))]
[RequireComponent(typeof(PlayerController))]

public class PlayerMovement : MonoBehaviour
{
    CharacterController characterController;
    [SerializeField] float walkingSpeed;
    [SerializeField] float runningSpeed;
    [SerializeField] float sprintSpeed;
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
    LockOn lockOn;
    float dropSpeed = 0;
    public bool movementFrozen = false;
    public bool rotationFrozen = false;
    public bool movementReduced = false;
    public float Gravity
    {
        get
        {
            if (characterController.isGrounded)
                dropSpeed = -1f;

            dropSpeed += Physics.gravity.y * Time.deltaTime;
            return dropSpeed * Time.deltaTime;
        }
    }
    void Awake()
    {
        characterController = GetComponentInChildren<CharacterController>();
        camera = Camera.main;
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        lockOn = GetComponent<LockOn>();

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            throw new MissingComponentException("Animator missing on player");
    }

    void Update()
    {
        movement.y = Gravity;
        HandleMovement();

        movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * movement; //handle camera rotation
        Quaternion movementForward;
        if (lockOn.IsLocked)      
        {
            Vector3 lockOnDirection = lockOn.TargetEnemy.position - characterController.transform.position;
            lockOnDirection.y = 0;
            direction = lockOnDirection;
        }

        if (!rotationFrozen && direction.magnitude > 0)
        {
            movementForward = Quaternion.LookRotation(direction, Vector3.up);
            characterController.transform.rotation = Quaternion.RotateTowards(characterController.transform.rotation, movementForward, turnSpeed * Time.deltaTime);
        }

        if (!movementFrozen)
            characterController.Move(movement);
    }

    void HandleMovement()
    {
        Vector2 movementInput = playerController.Move; 
        float movementMagnitude = movementInput.magnitude;
        
        if (movementFrozen)
        {
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
            animator.SetBool("IsMoving", false);
        }
        else if (movementMagnitude >= deadZone) 
        {
            animator.SetBool("IsMoving", true);
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
            if (movementMagnitude >= runThreshold && !movementReduced)
            {
                movementInput = runningSpeed * movementInput.normalized;
                animator.SetFloat("MovementSpeedMultiplier", runningSpeed / walkingSpeed);
            }
            else
            {
                movementInput = walkingSpeed * movementInput.normalized;
                animator.SetFloat("MovementSpeedMultiplier", 1);
            }

            if (lockOn.IsLocked)
            {
                Vector2 blending = movementInput.normalized;
                animator.SetFloat("MovementX", blending.x);
                animator.SetFloat("MovementY", blending.y);
            }
            else
            {
                animator.SetFloat("MovementX", 0);
                animator.SetFloat("MovementY", 1);
            }
        }
        else
        {
            animator.SetBool("IsMoving", false);
            movementInput = Vector2.zero;
        }

        movementInput *= Time.deltaTime;
        movement.x = movementInput.x;
        movement.z = movementInput.y;
    }
    /// <summary>
    /// Used when an action locks rotation, to sync orientation to input
    /// </summary>
    public void SyncRotation()
    {
        Vector2 movementInput = playerController.Move;
        if (movementInput.magnitude < deadZone) return;
        direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
        Quaternion movementForward = Quaternion.LookRotation(direction, Vector3.up);
        characterController.transform.rotation = movementForward;
    }
}