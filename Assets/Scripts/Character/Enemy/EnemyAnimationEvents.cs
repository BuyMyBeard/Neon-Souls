using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAnimationEvents : AnimationEvents
{
    Enemy enemy;
    NavMeshAgent agent;
    EnemyMeleeAttack meleeAttack;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
        agent = GetComponent<NavMeshAgent>();
        meleeAttack = GetComponent<EnemyMeleeAttack>();
    }
    public override void FreezeRotation() => enemy.rotationFrozen = true;
    public override void UnFreezeRotation() => enemy.rotationFrozen = false;
    public void ShootEvent() => GetComponent<ShooterEnemy>().Shoot();
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
        if (health.IsDead) return;
        UnFreezeMovement();
        RestoreTurnSpeed();
        UnFreezeRotation();
        enemy.EnableLockOn();
        meleeAttack.actionQueued = false;
        meleeAttack.StopFlickerBodyCollider();
    }
}
