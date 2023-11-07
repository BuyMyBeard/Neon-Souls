using System.Collections;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : Enemy
{

    enum CloseMovementType { StrafeLeft, StrafeRight, None }
    [SerializeField] WeightedAction<CloseMovementType>[] closeMovementChance;
    [SerializeField] float sprintingSpeed = 5;
    [SerializeField] float strafingSpeed = .7f;
    [SerializeField] float idealRange = 2;
    [SerializeField] float idealRangeTopMargin = .2f;
    [SerializeField] float idealRangeBottomMargin = .5f;
    CloseMovementType currentMovement = CloseMovementType.None;
    float timeSinceLastMovementChange = 0f;
    float TimeBeforeNextMovementChange;
    [SerializeField] float movementChangeTimeMin = 1;
    [SerializeField] float movementChangeTimeMax = 5;

    Direction moveDirection = Direction.None;

    bool OverMargin => DistanceFromPlayer > idealRange + idealRangeTopMargin;
    bool UnderMargin => DistanceFromPlayer < idealRange - idealRangeBottomMargin;
    bool InMargin => !OverMargin && !UnderMargin;
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
    enum Direction { In, Out, None}
    IEnumerator StayInIdeal()
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

    void PickRandomMovement()
    {
        timeSinceLastMovementChange = 0;
        TimeBeforeNextMovementChange = Random.Range(movementChangeTimeMin, movementChangeTimeMax);
        currentMovement = closeMovementChance.PickRandom();
    }
}