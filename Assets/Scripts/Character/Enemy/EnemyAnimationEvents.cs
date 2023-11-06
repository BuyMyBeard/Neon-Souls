using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationEvents : AnimationEvents
{
    Enemy enemy;
    NavMeshAgent agent;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
    }
    public override void FreezeRotation() => enemy.rotationFrozen = true;
    public override void UnFreezeRotation() => enemy.rotationFrozen = false;
    public void Shoot() => GetComponent<ShooterEnemy>().Shoot();
    public override void ChangeTurnSpeed(float turnSpeed) => enemy.turnSpeed = turnSpeed;
    public override void RestoreTurnSpeed() => enemy.RestoreTurnSpeed();
    public override void FreezeMovement()
    {
        base.FreezeMovement();
        agent.isStopped = true;
        enemy.movementFrozen = true;
    }
    public override void UnFreezeMovement()
    {
        base.UnFreezeMovement();
        agent.isStopped = false;
        enemy.movementFrozen = false;
    }

    public override void ResetAll()
    {
        base.ResetAll();
        UnFreezeMovement();
        RestoreTurnSpeed();
        UnFreezeRotation();
    }
}
