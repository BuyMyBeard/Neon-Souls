using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AgileElite : MeleeEnemy
{
    protected override void CloseMain()
    {
        timeSinceLastMovementChange += Time.deltaTime;
        if (movementFrozen) return;

        if (!rotationFrozen)
        {
            Quaternion towardsPlayer = Quaternion.LookRotation(DirectionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
        }

        if (timeSinceLastMovementChange > TimeBeforeNextMovementChange) PickRandomMovement();
        if (moveDirection == Direction.In)
        {
            agent.Move(agent.speed * Time.deltaTime * agent.transform.forward);
        }
        else if (moveDirection == Direction.Out)
        {
            agent.Move(-agent.speed * Time.deltaTime * agent.transform.forward);
        }
        else
        {
            switch (currentMovement)
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
}
