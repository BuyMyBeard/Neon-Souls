using System.Collections;
using UnityEngine;
[RequireComponent(typeof(LockOn))]
[RequireComponent(typeof(InputInterface))]
[RequireComponent(typeof(Stamina))]

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
    [SerializeField] float baseTurnSpeed = 100;
    [Range(0, 50)]
    [SerializeField] float acceleration = 5;
    [Range(0,50)]
    [SerializeField] float deceleration = 15;
    [Tooltip("Stamina/s")]
    [SerializeField] float sprintStaminaCost = 15;
    [SerializeField] float timeToConsiderFalling = .5f;
    float currentSpeed = 0;
    new Camera camera;
    Vector3 movement;
    Vector3 direction = Vector3.forward;
    InputInterface inputInterface;
    Animator animator;
    LockOn lockOn;
    Stamina stamina;
    PlayerAnimationEvents animationEvents;
    float dropSpeed = 0;
    public bool movementFrozen = false;
    public bool rotationFrozen = false;
    public bool movementReduced = false;
    float targetSpeed = 0;
    bool wasSprinting = false;
    bool isDecelerating = false;
    [HideInInspector] public float turnSpeed;
    Vector2 prevInput = Vector2.zero;
    bool isGrounded = false;
    [SerializeField] float groundCheckDistance = .8f;
    [SerializeField] LayerMask groundLayer;
    bool wasFalling = false;
    float timeSinceWasGrounded = 0;

    public bool IsSprinting { get; private set; } = false;
    Vector2 previousMovement = Vector2.zero;
    public float Gravity
    {
        get
        {
            dropSpeed += Physics.gravity.y * Time.deltaTime;
            return dropSpeed * Time.deltaTime;
        }
    }
    void Awake()
    {
        characterController = GetComponentInChildren<CharacterController>();
        camera = Camera.main;
        inputInterface = GetComponent<InputInterface>();
        animator = GetComponentInChildren<Animator>();
        lockOn = GetComponent<LockOn>();
        stamina = GetComponent<Stamina>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();

        animator = GetComponentInChildren<Animator>();
        if (animator == null)
            throw new MissingComponentException("Animator missing on player");
        turnSpeed = baseTurnSpeed;
    }

    void Update()
    {
        characterController.Move(new Vector3(0, Gravity, 0));
        isGrounded = characterController.isGrounded;
        bool isFalling = !isGrounded;

        if (characterController.isGrounded)
        {
            dropSpeed = -1f;
            timeSinceWasGrounded = 0;
        }
        else
        {
            timeSinceWasGrounded += Time.deltaTime;
            if (timeSinceWasGrounded > timeToConsiderFalling)
                isFalling = !(dropSpeed < 0 && Physics.Raycast(characterController.transform.position, Vector3.down, groundCheckDistance, groundLayer));
            else isFalling = false;
        }

        if (!isFalling && wasFalling)
        {
            animationEvents.ResetAll();
        }
        else if (isFalling && !wasFalling)
        {
            animationEvents.DisableActions();
            animationEvents.FreezeMovement();
        }

        wasFalling = isFalling;
        animator.SetBool("IsFalling", isFalling);

        HandleMovement();

        movement = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * movement; //handle camera rotation
        Quaternion movementForward;
        IsSprinting = inputInterface.IsSprinting && inputInterface.Move.magnitude >= runThreshold && stamina.CanRun && !movementReduced && animationEvents.ActionAvailable;
        if (lockOn.IsLocked && !IsSprinting)     
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

    // Someone refactor this pls :) I'm sowwwy
    void HandleMovement()
    {
        Vector2 movementInput = inputInterface.Move;
        if ((prevInput - movementInput).magnitude > 1.05)
        {
            movementInput = prevInput;
            prevInput = Vector2.zero;
        }
        else
            prevInput = movementInput;
        float movementMagnitude = movementInput.magnitude;
        IsSprinting = inputInterface.IsSprinting && movementMagnitude >= runThreshold && stamina.CanRun && !movementReduced && animationEvents.ActionAvailable;
        animator.SetBool("IsSprinting", IsSprinting);
        if (movementFrozen)
        {
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
            animator.SetBool("IsMoving", false);
        }
        else if (movementMagnitude >= deadZone) 
        {
            animator.SetBool("IsMoving", true);
            direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);


            if (IsSprinting)
            {
                stamina.Remove(sprintStaminaCost * Time.deltaTime, true);
                targetSpeed = targetSpeed > sprintSpeed ? sprintSpeed : targetSpeed + Time.deltaTime * acceleration;
                targetSpeed = targetSpeed > sprintSpeed ? sprintSpeed : targetSpeed;
            }
            else if (movementMagnitude >= runThreshold && !movementReduced)
            {
                targetSpeed = runningSpeed;
                animator.SetFloat("MovementSpeedMultiplier", runningSpeed / walkingSpeed);
            }
            else
            {
                targetSpeed = walkingSpeed;
                animator.SetFloat("MovementSpeedMultiplier", 1);
            }

            if (lockOn.IsLocked && !inputInterface.IsSprinting)
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
            targetSpeed = 0;
            animator.SetBool("IsMoving", false);
        }

        if (wasSprinting && !IsSprinting) StartCoroutine(Decelerate());
        wasSprinting = IsSprinting;
        if (!isDecelerating)
        {
            currentSpeed = targetSpeed;
            movementInput = currentSpeed * Time.deltaTime * movementInput.normalized;
        }
        else if (movementMagnitude < deadZone)
        {
            movementInput = currentSpeed * Time.deltaTime * previousMovement;
        }
        else
            movementInput = currentSpeed * Time.deltaTime * movementInput.normalized;
        movement.x = movementInput.x;
        movement.z = movementInput.y;
        if (!movementFrozen && movementMagnitude >= deadZone)
            previousMovement = movementInput.normalized;
    }
    /// <summary>
    /// Used when an action locks rotation, to sync orientation to input
    /// </summary>
    public void SyncRotation()
    {
        Vector2 movementInput = inputInterface.Move;
        if (movementInput.magnitude < deadZone) return;
        direction = Quaternion.Euler(0, camera.transform.eulerAngles.y, 0) * new Vector3(movementInput.x, 0, movementInput.y);
        Quaternion movementForward = Quaternion.LookRotation(direction, Vector3.up);
        characterController.transform.rotation = movementForward;
    }
    IEnumerator Decelerate()
    {
        isDecelerating = true;
        while (targetSpeed < currentSpeed)
        {
            yield return null;
            currentSpeed -= Time.deltaTime * deceleration; 
        }
        currentSpeed = targetSpeed;
        isDecelerating = false;
    }

    public void RestoreTurnSpeed() => turnSpeed = baseTurnSpeed;
}