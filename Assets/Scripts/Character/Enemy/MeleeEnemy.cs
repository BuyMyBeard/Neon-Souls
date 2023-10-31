using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class MeleeEnemy : Enemy
{
    NavMeshAgent agent;
    [SerializeField] float turnSpeed;

    protected override void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        base.Awake();
    }
    protected override void IdleInit()
    {
        agent.ResetPath();
    }
    protected override void InRangeInit()
    {
        agent.enabled = true;
        agent.updateRotation = true;
    }
    protected override void InRangeMain()
    {
        agent.SetDestination(Target.position);
    }
    protected override void InRangeExit()
    {
        agent.ResetPath();
    }
    protected override void CloseMain()
    {
        Quaternion towardsPlayer = Quaternion.LookRotation(-DistanceFromPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
    }
}