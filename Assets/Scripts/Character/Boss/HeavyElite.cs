using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class HeavyElite : MeleeEnemy
{
    protected override void InRangeInit()
    {
        inRangeInitEvent.Invoke();
        agent.enabled = true;
        agent.updateRotation = true;
        agent.speed = BaseSpeed;
    }
    protected override void InRangeMain()
    {
        agent.SetDestination(Target.position);
        animator.SetBool("IsWalking", Velocity.magnitude > 0);
    }
    protected override void InRangeExit()
    {
        base.InRangeExit();
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsWalking", false);
        agent.ResetPath();
        RestoreSpeed();
    }
}
