using System.Collections;
using UnityEditor.UIElements;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public class ShooterEnemy : Enemy
{
    // InRange
    [SerializeField] string poolTag = "BulletPool";
    [SerializeField] ObjectPool pool;
    // [SerializeField] Transform arm;
    [SerializeField] Transform gunMuzzle;
    [SerializeField] float shootCooldown = 5;
    [SerializeField] float getAwayDistance = 5;

    protected override void Awake()
    {
        base.Awake();

        var existingPool = GameObject.FindGameObjectWithTag(poolTag);
        pool = existingPool != null ? existingPool.GetComponent<ObjectPool>() : Instantiate(pool);
    }
    protected override void InRangeExit()
    {
        base.InRangeExit();
        StopCoroutine(nameof(Aim));
    }

    protected override void InRangeInit()
    {
        base.InRangeInit();
        StartCoroutine(nameof(Aim));
    }
    protected override void InRangeMain()
    {
        base.InRangeMain();
        if (!rotationFrozen)
        {
            Quaternion towardsPlayer = Quaternion.LookRotation(-DirectionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
        }

       // var armTowardsPlayer = Quaternion.LookRotation(Target.position - arm.position);
       // arm.rotation = Quaternion.RotateTowards(arm.rotation, armTowardsPlayer, turnSpeed * Time.deltaTime);
    }

    protected override void CloseMain()
    {
        base.CloseMain();
        if (!movementFrozen && enemyAnimationEvents.ActionAvailable)
        {
            agent.Move(-BaseSpeed * Time.deltaTime * transform.forward);
            if (DistanceFromPlayer > getAwayDistance) ChangeMode(ModeId.InRange);
        }
    }
    
    IEnumerator Aim()
    {
        while (true)
        {
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);animator.SetTrigger("Shoot");
            enemyAnimationEvents.DisableActions();
            enemyAnimationEvents.FreezeMovement();
            
            yield return new WaitForSeconds(shootCooldown);
        }
    }

    public void Shoot()
    {
        var bullet = pool.SpawnObject(gunMuzzle, out Coroutine p_returnCoroutine).GetComponent<Bullet>();
        bullet.target = Target;
        bullet.p_returnCoroutine = p_returnCoroutine;
    }
}
