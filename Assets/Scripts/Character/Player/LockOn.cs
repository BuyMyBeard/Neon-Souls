using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    [SerializeField, Range(0, 360)] float viewRadius = 90;
    [SerializeField, Range(0, 360)] float viewAngle = 60;
    [SerializeField] LayerMask lockableMask;
    [SerializeField] LayerMask environmentMask;
    [SerializeField] float detectionRefresh = 0.2f;
    [SerializeField] float yAxisLockOffset = 1.5f;
    [SerializeField, Range(1,2)] float maxLockOnDistance;
    [SerializeField, Range(0, 5)] float healthbarLingerTimeOnEnemyDeath = 1.5f; 
    readonly List<Transform> enemiesInSight = new();
    EnemyHealthbar enemyHealthbar;
    Health enemyHealth = null;
    Transform player;
    Transform camFollowTarget;
    bool isSmoothLooking = false;
    public bool IsLocked { get; private set; } = false;
    public Transform TargetEnemy { get; private set; } = null;

    public void Awake()
    {
        player = GetComponentInChildren<CharacterController>().transform;
        camFollowTarget = GameObject.Find("FollowTarget").transform;
        enemyHealthbar = FindObjectOfType<EnemyHealthbar>();
    }
    public void Start()
    {
        StartCoroutine(FindEnemiesCoroutine());
    }
    void OnLockOn()
    {
        if (IsLocked)
        {
            IsLocked = false;
            enemyHealthbar.Hide();
            enemyHealth = null;
        }
        else if (enemiesInSight.Count > 0)
        {
            IsLocked = true;
            TargetEnemy = FindClosestEnemy();
            enemyHealth = TargetEnemy.gameObject.GetComponentInParent<Health>();
            if (enemyHealth != null)
            {
                enemyHealthbar.trackedEnemy = enemyHealth;
                enemyHealth.displayHealthbar = enemyHealthbar;
                enemyHealthbar.Set(enemyHealth.CurrentHealth, enemyHealth.MaxHealth);
                enemyHealthbar.Show();
            }

            Debug.DrawLine(camFollowTarget.position, TargetEnemy.position,Color.blue,1);
            StartCoroutine(SmoothLook(Quaternion.LookRotation(new Vector3(TargetEnemy.position.x, TargetEnemy.position.y - yAxisLockOffset, TargetEnemy.position.z) - camFollowTarget.position)));
            StartCoroutine(CamLockedOnTarget(TargetEnemy));
        }
        else
            StartCoroutine(SmoothLook(Quaternion.LookRotation(player.forward)));
    }
    IEnumerator CamLockedOnTarget(Transform targetEnemy)
    {
        while (IsLocked)
        {
            if (enemyHealth != null && enemyHealth.IsDead)
            {
                IsLocked = false;
                yield return new WaitForSeconds(healthbarLingerTimeOnEnemyDeath);
                if (!IsLocked)
                    enemyHealthbar.Hide();
                yield break;
            }
            if (isSmoothLooking)
            {
                yield return null;
                continue;
            }
            camFollowTarget.LookAt(new Vector3(targetEnemy.position.x, targetEnemy.position.y - yAxisLockOffset, targetEnemy.position.z));

            if(Vector3.Distance(camFollowTarget.position, targetEnemy.position) > viewRadius * maxLockOnDistance)
            {
                IsLocked = false;
                enemyHealthbar.Hide();
            }
            yield return null;
        }
    }
    // This function was inspired by Sebastian Lague
    // https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague

    // (For Yannick) I refactored the shit out of this, you can check it out but it should be good   - Alex
    void FindEnemiesInSight()
    {
        enemiesInSight.Clear();

        //If we encounter performance issues, we could use NonAlloc and reuse an array
        Collider[] enemiesInViewRadius = Physics.OverlapSphere(camFollowTarget.position, viewRadius, lockableMask);
        foreach (var enemyCollider in enemiesInViewRadius)
        {
            Transform enemy = enemyCollider.transform;
            Health sightedEnemyHealth = enemy.GetComponentInParent<Health>();

            //Prevents locking onto enemy which has no health
            if (sightedEnemyHealth != null && sightedEnemyHealth.IsDead)
                continue;

            Vector3 directionToEnemy = (enemy.position - Camera.main.transform.position).normalized;
            float distanceToTarget = Vector3.Distance(Camera.main.transform.position, enemy.position);
            
            if (EnemyInCamAngle(directionToEnemy) && EnemyInRangeAndSight(directionToEnemy, distanceToTarget))
            {
                enemiesInSight.Add(enemy);
                Debug.DrawRay(Camera.main.transform.position, directionToEnemy * distanceToTarget, Color.magenta, 1);
            }
        }
    }

    bool EnemyInCamAngle(Vector3 directionToEnemy) => Vector3.Angle(Camera.main.transform.forward, directionToEnemy) < viewAngle / 2;
    bool EnemyInRangeAndSight(Vector3 directionToEnemy, float distanceToTarget) => !Physics.Raycast(Camera.main.transform.position, directionToEnemy, distanceToTarget, environmentMask);
    IEnumerator FindEnemiesCoroutine()
    {
        while (true)
        {
            FindEnemiesInSight();
            yield return new WaitForSeconds(detectionRefresh);
        }
    }
    IEnumerator SmoothLook(Quaternion at)
    {
        isSmoothLooking = true;
        float elapsedTime = 0f;
        const float LERPTIME = 0.05f;
        Quaternion initialRotation = camFollowTarget.rotation;
        while (elapsedTime < LERPTIME)
        {
            camFollowTarget.rotation = Quaternion.Lerp(initialRotation, at, elapsedTime / LERPTIME);
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        isSmoothLooking = false;
    }
    Transform FindClosestEnemy() 
    {
        return enemiesInSight.Aggregate((e1, e2) => (e1.transform.position - camFollowTarget.transform.position).magnitude < 
               (e2.transform.position - camFollowTarget.transform.position).magnitude ? e1 : e2);
    }
    void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(camFollowTarget.position, viewRadius);
        Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up);
        Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle / 2, Vector3.up);
        Vector3 leftRayDirection = leftRayRotation * camFollowTarget.forward;
        Vector3 rightRayDirection = rightRayRotation * camFollowTarget.forward;
        Gizmos.color = Color.green;
        Gizmos.DrawRay(Camera.main.transform.position, leftRayDirection * viewRadius);
        Gizmos.DrawRay(Camera.main.transform.position, rightRayDirection * viewRadius);
    }
}
