using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class EnemyMeleeAttack : MeleeAttack
{
    enum EnemyAction { SliceOverHead, BackhandSlice, Attack3, Block };
    float timeSinceLastAction = 0;
    EnemyAnimationEvents enemyAnimationEvents;
    Enemy enemy;
    [SerializeField] AttackDef fullBody;

    [SerializeField] WeightedAction<EnemyAction>[] possibleActions;
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
                Debug.Log("Attack1");
                animator.SetTrigger("SliceOverHead");
                enemyAnimationEvents.FreezeMovement();
                enemyAnimationEvents.ChangeTurnSpeed(100);
                enemyAnimationEvents.DisableActions();
                break;

            case EnemyAction.BackhandSlice:
                animator.SetTrigger("BackhandSlice");
                enemyAnimationEvents.FreezeMovement();
                enemyAnimationEvents.DisableActions();
                Debug.Log("Attack2");
                break;

            case EnemyAction.Attack3:
                Debug.Log("Attack3");
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
