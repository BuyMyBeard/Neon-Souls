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
    [SerializeField] Transform arm;
    [SerializeField] Transform gunSight;
    [SerializeField] float shootCooldown;
    [SerializeField] float turnSpeed;
    [SerializeField] float shootingAngleMin = 5f;

    // Close
    NavMeshAgent agent;

    // Coroutine handling helpers
    bool shouldEndShootCoroutine = false;
    Coroutine shoot;

    protected override void Awake()
    {
        base.Awake();
        agent = GetComponent<NavMeshAgent>();

        var existingPool = GameObject.FindGameObjectWithTag(poolTag);
        pool = existingPool != null ? existingPool.GetComponent<ObjectPool>() : Instantiate(pool);
    }
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
        Quaternion towardsPlayer = Quaternion.LookRotation(-DistanceFromPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);

        var armTowardsPlayer = Quaternion.LookRotation(Target.position - arm.position);
        arm.rotation = Quaternion.RotateTowards(arm.rotation, armTowardsPlayer, turnSpeed * Time.deltaTime);
    }

    protected override void CloseMain()
    {
        
    }
    IEnumerator StartShoot()
    {
        yield return new WaitUntil(() => shoot == null);
        if (Mode.Id != ModeId.InRange) yield break;
        shoot = StartCoroutine(ShootCoroutine());
    }
    IEnumerator ShootCoroutine()
    {
        while (true)
        {
            if (shouldEndShootCoroutine)
            {
                shouldEndShootCoroutine = false;
                shoot = null;
                yield break;
            }

            float towardsPlayer;
            float currentRotation;
            yield return new WaitUntil(() => {
                towardsPlayer = Quaternion.LookRotation(-DistanceFromPlayer, Vector3.up).eulerAngles.y;
                currentRotation = transform.rotation.eulerAngles.y;
                if (towardsPlayer > 180f) towardsPlayer = 360f - towardsPlayer;
                if (currentRotation > 180f) currentRotation = 360f - currentRotation;
                //Debug.Log($"{towardsPlayer} - {currentRotation}");
                return (towardsPlayer - currentRotation) < shootingAngleMin;
            });
            Shoot();
            yield return new WaitForSeconds(shootCooldown);
        }
    }

    void Shoot()
    {
        var bullet = pool.SpawnObject(gunSight, out Coroutine p_returnCoroutine).GetComponent<Bullet>();
        bullet.target = Target;
        bullet.p_returnCoroutine = p_returnCoroutine;
    }
}
