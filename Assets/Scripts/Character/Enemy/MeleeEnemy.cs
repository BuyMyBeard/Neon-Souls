using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : Enemy
{
    protected enum CloseMovementType { StrafeLeft, StrafeRight, None }
    [SerializeField] protected WeightedAction<CloseMovementType>[] closeMovementChance;
    [SerializeField] protected float sprintingSpeed = 5;
    [SerializeField] protected float strafingSpeed = .7f;
    [SerializeField] protected float idealRange = 2;
    [SerializeField] protected float idealRangeTopMargin = .2f;
    [SerializeField] protected float idealRangeBottomMargin = .5f;
    protected CloseMovementType currentMovement = CloseMovementType.None;
    protected float timeSinceLastMovementChange = 0f;
    protected float TimeBeforeNextMovementChange;
    [SerializeField] protected float movementChangeTimeMin = 1;
    [SerializeField] protected float movementChangeTimeMax = 5;

    protected Direction moveDirection = Direction.None;

    protected bool OverMargin => DistanceFromPlayer > idealRange + idealRangeTopMargin;
    protected bool UnderMargin => DistanceFromPlayer < idealRange - idealRangeBottomMargin;
    protected bool InMargin => !OverMargin && !UnderMargin;
    protected override void Update()
    {
        base.Update();
    }
    protected override void Awake()
    {
        base.Awake();
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
        agent.speed = sprintingSpeed;
    }
    protected override void InRangeMain()
    {
        base.InRangeMain();
        agent.SetDestination(Target.position);
        animator.SetBool("IsSprinting", Velocity.magnitude > 0);
    }
    protected override void InRangeExit()
    {
        base.InRangeExit();
        animator.SetBool("IsMoving", false);
        agent.ResetPath();
        animator.SetBool("IsSprinting", false);
        RestoreSpeed();
    }

    protected override void CloseInit()
    {
        base.CloseInit();
        PickRandomMovement();
        StartCoroutine(nameof(StayInIdeal));
    }
    protected override void CloseExit()
    {
        base.CloseExit();
        StopCoroutine(nameof(StayInIdeal));
    }
    protected override void CloseMain()
    {
        timeSinceLastMovementChange += Time.deltaTime;
        base.CloseMain();
        if (movementFrozen) return;

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
    protected enum Direction { In, Out, None}
    protected IEnumerator StayInIdeal()
    {
        moveDirection = Direction.None;
        while (true)
        {
            yield return new WaitUntil(() => !InMargin && enemyAnimationEvents.ActionAvailable);
            moveDirection = OverMargin ? Direction.In : Direction.Out;
            yield return new WaitUntil(() =>
            {
                if (moveDirection == Direction.Out)
                    return DistanceFromPlayer > idealRange;

                return DistanceFromPlayer < idealRange;
            });
            moveDirection = Direction.None;
        }
    }

    protected void PickRandomMovement()
    {
        timeSinceLastMovementChange = 0;
        TimeBeforeNextMovementChange = Random.Range(movementChangeTimeMin, movementChangeTimeMax);
        currentMovement = closeMovementChance.PickRandom();
    }
}