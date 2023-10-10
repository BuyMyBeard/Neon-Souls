using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Pool;

[RequireComponent(typeof(NavMeshAgent))]
public class ShooterEnemy : Enemy
{
    // InRange
    [SerializeField] ObjectPool pool;
    [SerializeField] Transform arm;
    [SerializeField] Transform gunSight;
    [SerializeField] float shootCooldown;
    [SerializeField] float turnSpeed;
    [SerializeField] float shootingAngleMin = 5f;

    // Close
    NavMeshAgent agent;


    bool shouldEndShootCoroutine = false;
    Coroutine shoot;

    protected override void InRangeExit()
    {
        shouldEndShootCoroutine = true;
    }

    protected override void InRangeInit()
    {
        StartCoroutine(StartShoot());
    }
    protected override void InRangeMain()
    {
        Quaternion towardsPlayer = Quaternion.LookRotation(-posDifference, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);

        var armTowardsPlayer = Quaternion.LookRotation(target.position - arm.position);
        arm.rotation = Quaternion.RotateTowards(arm.rotation, armTowardsPlayer, turnSpeed * Time.deltaTime);
    }

    protected override void CloseMain()
    {
        
    }
    IEnumerator StartShoot()
    {
        yield return new WaitUntil(() => shoot == null);
        if (Mode.Id != ModeId.InRange) yield break;
        Debug.Log("StartShoot()");
        shoot = StartCoroutine(ShootCoroutine());
    }
    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (shouldEndShootCoroutine)
            {
                Debug.Log("shouldEndShootCoroutine");
                shouldEndShootCoroutine = false;
                shoot = null;
                yield break;
            }

            float towardsPlayer;
            float currentRotation;
            yield return new WaitUntil(() => {
                towardsPlayer = Quaternion.LookRotation(posDifference, Vector3.up).eulerAngles.y - 180f;
                currentRotation = transform.rotation.eulerAngles.y;
                if (towardsPlayer > 180f) towardsPlayer = 360f - towardsPlayer;
                if (currentRotation > 180f) currentRotation = 360f - currentRotation;
                Debug.Log($"{towardsPlayer} - {currentRotation}");
                return (towardsPlayer - currentRotation) < shootingAngleMin;
            });
            Shoot();
            yield return new WaitForSeconds(shootCooldown);
        }
    }

    void Shoot()
    {
        var bullet = pool.SpawnObject(gunSight, out Coroutine p_returnCoroutine).GetComponent<Bullet>();
        bullet.target = target;
        bullet.p_returnCoroutine = p_returnCoroutine;
    }
}
