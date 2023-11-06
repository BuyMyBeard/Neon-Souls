using System.Collections;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : Enemy
{
    public Vector3 Velocity { get; private set; }
    Vector3 prevPosition;

    protected override void Update()
    {
        base.Update();
        AnimateMovement();
        Velocity = (transform.position - prevPosition) / Time.deltaTime;
        prevPosition = transform.position;
    }
    protected override void Awake()
    {
        base.Awake();
        prevPosition = transform.position;
    }
    protected override void IdleInit()
    {
        base.IdleInit();
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
        agent.SetDestination(Target.position);
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
    }

    protected void AnimateMovement()
    {
        if (!enemyAnimationEvents.ActionAvailable) return;

        animator.SetBool("IsMoving", Velocity.magnitude > 0);
        Vector3 relativeVelocity = transform.InverseTransformDirection(Velocity);
        Vector2 flatRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z).normalized;
        animator.SetFloat("MovementX", flatRelativeVelocity.x);
        animator.SetFloat("MovementY", flatRelativeVelocity.y);
    }
}