using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileElite : MeleeEnemy
{
    [SerializeField] Enemy HeavyBoss;
    [SerializeField] float minDistanceFromHeavy = 3;

    float DistanceFromHeavy => HeavyBoss != null ? Vector3.Distance(transform.position, HeavyBoss.transform.position) : float.MaxValue;

    protected override void CloseMain()
    {
        timeSinceLastMovementChange += Time.deltaTime;

        if (!rotationFrozen)
        {
            Quaternion towardsPlayer = Quaternion.LookRotation(DirectionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
        }

        if (movementFrozen) return;

        if (moveDirection == Direction.In)
        {
            agent.Move(agent.speed * Time.deltaTime * agent.transform.forward);
        }
        else if (moveDirection == Direction.Out)
        {
            agent.Move(-agent.speed * Time.deltaTime * agent.transform.forward);
        }
        if (DistanceFromHeavy < minDistanceFromHeavy)
        {
            
            Vector3 directionToHeavy = HeavyBoss.transform.position - transform.position;
            directionToHeavy.y = 0;
            directionToHeavy = directionToHeavy.normalized;

            CloseMovementType strafeDirection = Vector3.Dot(transform.right, directionToHeavy) > 0 ? CloseMovementType.StrafeLeft : CloseMovementType.StrafeRight;
               
            switch (strafeDirection)
            {
                case CloseMovementType.StrafeLeft:
                    agent.Move(-strafingSpeed * Time.deltaTime * agent.transform.right);
                    break;

                case CloseMovementType.StrafeRight:
                    agent.Move(strafingSpeed * Time.deltaTime * agent.transform.right);
                    break;

                default:
                    break;
            }
        }
    }
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
        animator.SetBool("IsMoving", Velocity.magnitude > 0);
    }
    protected override void InRangeExit()
    {
        base.InRangeExit();
        animator.SetBool("IsMoving", false);
        agent.ResetPath();
        RestoreSpeed();
    }
}
