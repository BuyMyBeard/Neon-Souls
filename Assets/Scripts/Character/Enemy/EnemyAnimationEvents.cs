using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyAnimationEvents : AnimationEvents
{
    Enemy enemy;
    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();

    }
    public override void FreezeRotation() => enemy.rotationFrozen = true;
    public override void UnFreezeRotation() => enemy.rotationFrozen = false;
    public void Shoot() => GetComponent<ShooterEnemy>().Shoot();
    public override void ChangeTurnSpeed(float turnSpeed) => enemy.turnSpeed = turnSpeed;
    public override void RestoreTurnSpeed() => enemy.RestoreTurnSpeed();

}
