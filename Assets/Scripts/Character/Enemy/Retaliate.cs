using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Retaliate : MonoBehaviour
{
    [SerializeField] WeightedAction<EnemyAction>[] possibleRetaliations;
    [SerializeField] float retaliationRange;
    Animator animator;
    EnemyAnimationEvents enemyAnimationEvents;
    Stagger stagger;
    Enemy enemy;
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        animator = GetComponent<Animator>();
        stagger = GetComponent<Stagger>();
        enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        stagger.onStagger.AddListener(StartLookForRetaliation);
    }
    
    void StartLookForRetaliation()
    {
        StopCoroutine(LookForRetaliation());
        StartCoroutine(LookForRetaliation());
    }

    IEnumerator LookForRetaliation()
    {
        yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
        if (enemy.DistanceFromPlayer < retaliationRange)
            DoRetaliation();

        if (enemy.Mode.Id == Enemy.ModeId.Idle)
        {
            if (enemy.DistanceFromPlayer < retaliationRange)
                enemy.ChangeMode(Enemy.ModeId.Close);
            else
                enemy.ChangeMode(Enemy.ModeId.InRange);
        }       
    }

    void DoRetaliation()
    {
        EnemyAction action = possibleRetaliations.PickRandom();
        switch (action)
        {
            case EnemyAction.None:
            case EnemyAction.Block:
                break;

            case EnemyAction.GunAttack:
                animator.SetTrigger(action.ToString());
                enemyAnimationEvents.DisableActions();
                enemyAnimationEvents.FreezeMovement();
                break;

            case EnemyAction.RollAttack:
                animator.SetTrigger(action.ToString());
                enemy.transform.rotation = Quaternion.LookRotation(enemy.DirectionToPlayer, Vector3.up);
                enemyAnimationEvents.DisableActions();
                enemyAnimationEvents.FreezeRotation();
                enemyAnimationEvents.FreezeMovement();
                break;

            default:
                enemyAnimationEvents.DisableActions();
                enemyAnimationEvents.FreezeMovement();
                animator.SetTrigger(action.ToString());
                break;
        }
    }
}
