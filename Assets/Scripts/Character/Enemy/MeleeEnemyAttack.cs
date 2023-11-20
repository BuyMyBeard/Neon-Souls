using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum EnemyAction { None, SliceOverHead, BackhandSlice, SpinAttack, MaleniaAttack, Block, RollAttack, GunAttack, Shoot, AgileEliteAttack, HeavyEliteAttack, PunchSlice };
public class EnemyMeleeAttack : MeleeAttack
{
    float timeSinceLastAction = 0;
    EnemyAnimationEvents enemyAnimationEvents;
    Enemy enemy;
    [SerializeField] AttackDef fullBody;

    [SerializeField] WeightedAction<EnemyAction>[] possibleActions;
    [SerializeField] WeightedAction<EnemyAction>[] initiateActions;
    [Range(0f, 10f)]
    [SerializeField] float minActionCooldown = 1;
    [Range(0f, 10f)]
    [SerializeField] float maxActionCooldown = 2;
    protected override void Awake()
    {
        base.Awake();
        enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        enemy = GetComponent<Enemy>();
    }

    protected override void Start()
    {
        base.Start();
        enemy.closeInitEvent.AddListener(StartLookForAttack);
        enemy.closeExitEvent.AddListener(StopLookForAttack);
    }
    protected void Update()
    {
        timeSinceLastAction += Time.deltaTime;
    }
    private void StartLookForAttack() => StartCoroutine(nameof(LookForAttack));
    private void StopLookForAttack() => StopCoroutine(nameof(LookForAttack));
    IEnumerator LookForAttack()
    {
        DoAction(initiateActions.PickRandom());
        yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
        while (true)
        {
            timeSinceLastAction = 0;
            float timeBeforeNextAction = UnityEngine.Random.Range(minActionCooldown, maxActionCooldown);
            yield return new WaitUntil(() =>
            {
                if (!enemyAnimationEvents.ActionAvailable) return false;
                return timeSinceLastAction >= timeBeforeNextAction;
            });
            DoAction(possibleActions.PickRandom());
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
        }
    }
    private void DoAction(EnemyAction action)
    {
        switch(action)
        {
            case EnemyAction.SliceOverHead:
                animator.SetTrigger(action.ToString());
                enemyAnimationEvents.FreezeMovement();
                enemyAnimationEvents.ChangeTurnSpeed(100);
                enemyAnimationEvents.DisableActions();
                break;

            case EnemyAction.SpinAttack:
            case EnemyAction.HeavyEliteAttack:
                animator.SetTrigger(action.ToString());
                enemyAnimationEvents.FreezeMovement();
                enemyAnimationEvents.FreezeRotation();
                enemyAnimationEvents.DisableActions();
                break;

            case EnemyAction.BackhandSlice:
            case EnemyAction.MaleniaAttack:
            case EnemyAction.GunAttack:
            case EnemyAction.Shoot:
            case EnemyAction.AgileEliteAttack:
            case EnemyAction.PunchSlice:
                animator.SetTrigger(action.ToString());
                enemyAnimationEvents.FreezeMovement();
                enemyAnimationEvents.DisableActions();
                break;

            default:
                break;
        }
    }

    public void StartFlickerBodyCollider() => StartCoroutine(nameof(FlickerBodyCollider));
    public void StopFlickerBodyCollider() => StopCoroutine(nameof(FlickerBodyCollider));

    private IEnumerator FlickerBodyCollider()
    {
        const float FlickerFrequency = .3f;
        while (true)
        {
            enemyAnimationEvents.InitWeaponCollider(fullBody);
            yield return new WaitForSeconds(FlickerFrequency);
            enemyAnimationEvents.DisableWeaponCollider(fullBody);
            yield return null;
        }
    }
}
