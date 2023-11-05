using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;


public class EnemyMeleeAttack : MeleeAttack
{
    enum Action { Attack1, Attack2, Attack3 };

    float timeSinceLastAction = 0;
    EnemyAnimationEvents enemyAnimationEvents;
    Enemy enemy;

    [SerializeField] AttackType[] possibleActions;
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

    private void OnEnable()
    {
        enemy.closeInitEvent.AddListener(StartLookForAttack);
        enemy.closeExitEvent.AddListener(StopLookForAttack);
    }
    private void OnDisable()
    {
        enemy.closeInitEvent.RemoveListener(StartLookForAttack);
        enemy.closeExitEvent.RemoveListener(StopLookForAttack);
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
            DoAction(PickRandomAction());
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
        }
    }

    private Action PickRandomAction()
    {
        float totalWeight = possibleActions.Sum(action => action.weight);
        List<(float, Action)> chanceList = new();
        float current = 0;
        foreach (var action in possibleActions)
        {
            current += action.weight;
            chanceList.Add((current, action.actionName));
        }
        float pickedNumber = UnityEngine.Random.Range(0, totalWeight);
        return chanceList.Find(action => pickedNumber <= action.Item1).Item2;
    }
    private void DoAction(Action action)
    {
        switch(action)
        {
            case Action.Attack1:
                Debug.Log("Attack1");
                animator.SetTrigger("SliceOverHead");
                enemyAnimationEvents.ChangeTurnSpeed(100);
                enemyAnimationEvents.DisableActions();
                break;

            case Action.Attack2:
                animator.SetTrigger("BackhandSlice");
                enemyAnimationEvents.DisableActions();
                Debug.Log("Attack2");
                break;

            case Action.Attack3:
                Debug.Log("Attack3");
                break;
        }
    }
    [Serializable]
    struct AttackType
    {
        public AttackType(Action actionName, float weight = 1)
        {
            this.actionName = actionName;
            this.weight = weight;
        }
        public Action actionName;
        public float weight; 
    }
}
