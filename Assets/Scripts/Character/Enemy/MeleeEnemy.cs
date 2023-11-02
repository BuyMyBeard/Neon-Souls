using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : Enemy
{
    [SerializeField] float turnSpeed;
    public Vector3 Velocity { get; private set; }
    Vector3 prevPosition;
    void Start()
    {
        prevPosition = transform.position;
    }
    protected override void Update()
    {
        base.Update();
        Velocity = (transform.position - prevPosition) / Time.deltaTime;
    }
    protected override void Awake()
    {
        base.Awake();
    }
    protected override void IdleInit()
    {
        base.IdleInit();
        agent.ResetPath();
    }
    protected override void InRangeInit()
    {
        base.InRangeInit();
        agent.enabled = true;
        agent.updateRotation = true;
    }
    protected override void InRangeMain()
    {
        base.InRangeMain();
        agent.Move(Time.deltaTime * 3 * Vector3.left);
        agent.SetDestination(Target.position);
        AnimateMovement();
    }
    protected override void InRangeExit()
    {
        base.InRangeExit();
        animator.SetBool("IsMoving", false);
        agent.ResetPath();
    }
    protected override void CloseMain()
    {
        base.CloseMain();
        Quaternion towardsPlayer = Quaternion.LookRotation(-DistanceFromPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
    }

    protected void AnimateMovement()
    {
        animator.SetBool("IsMoving", Velocity.magnitude > 0);
        Vector3 relativeVelocity = transform.InverseTransformDirection(Velocity);
        Vector2 flatRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z).normalized;
        animator.SetFloat("MovementX", flatRelativeVelocity.x);
        animator.SetFloat("MovementY", flatRelativeVelocity.y);
    }
}