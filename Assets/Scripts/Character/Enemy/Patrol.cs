using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Enemy))]
[RequireComponent(typeof(NavMeshAgent))]
public class Patrol : MonoBehaviour
{
    [SerializeField] Task[] tasks;
    Enemy enemy;
    NavMeshAgent agent;
    [SerializeField] float minRemainingDistance = .1f;

    [Serializable]
    public struct Task
    {
        public PatrolPoint patrolPoint;
        public float waitTime;
    }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
    }
    private void Start()
    {
        enemy.idleInitEvent.AddListener(StartPatrolling);
        enemy.idleExitEvent.AddListener(StopPatrolling);
    }
    private void StartPatrolling() => StartCoroutine(nameof(PatrolAround));
    private void StopPatrolling() => StopCoroutine(nameof(PatrolAround));
    IEnumerator PatrolAround()
    {
        int i = 0;
        while (true)
        {
            Task currentTask = tasks[i];
            agent.SetDestination(currentTask.patrolPoint.transform.position);
            yield return null;
            yield return new WaitUntil(() => agent.remainingDistance < minRemainingDistance);
            agent.ResetPath();
            if (currentTask.waitTime < 0) break;
            yield return new WaitForSeconds(currentTask.waitTime);
            i = (i + 1) % tasks.Length;
        }
    }
}
