using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class LockOn : MonoBehaviour
{
    bool isLocked = false;
    [SerializeField] float viewRadius = 90;
    [SerializeField, Range(0, 360)] float viewAngle = 60;
    [SerializeField] LayerMask lockableMask;
    [SerializeField] LayerMask environmentMask;
    [SerializeField] float detectionRefresh = 0.2f;
    [SerializeField] float yAxisLockOffset = 1.5f;
    [SerializeField, Range(1,2)] 
    float maxLockOnDistance;
    List<Transform> enemiesInSight = new();
    Transform targetEnemy = null;
    EnemyHealthbar enemyHealthbar;
    Transform player;
    Transform camFollowTarget;
    bool isSmoothLooking = false;

    public void Awake()
    {
        player = GameObject.Find("PlayerPlaceHolder").transform; // a changer pour le nom du vrai model.
        camFollowTarget = GameObject.FindGameObjectWithTag("FollowTarget").transform;
        enemyHealthbar = FindObjectOfType<EnemyHealthbar>();

    }
    public void Start()
    {
        StartCoroutine(FindEnemiesCoroutine());
    }
    void OnLockOn()
    {
        if (isLocked)
        {
            isLocked = false;
            enemyHealthbar.Hide();

        }
        else if (!isLocked && enemiesInSight.Count > 0)
        {
            isLocked = true;
            targetEnemy = FindClosestEnemy();
            Health enemyHealth = targetEnemy.gameObject.GetComponentInParent<Health>();
            enemyHealthbar.trackedEnemy = enemyHealth;
            enemyHealth.displayHealthbar = enemyHealthbar;
            enemyHealthbar.Set(enemyHealth.CurrentHealth, enemyHealth.MaxHealth);
            enemyHealthbar.Show();

            Debug.DrawLine(camFollowTarget.position, targetEnemy.position,Color.blue,1);
            StartCoroutine(SmoothLook(Quaternion.LookRotation(new Vector3(targetEnemy.position.x, targetEnemy.position.y - yAxisLockOffset, targetEnemy.position.z) - camFollowTarget.position)));
            StartCoroutine(CamLockedOnTarget(targetEnemy));
        }
        else
        {
            StartCoroutine(SmoothLook(Quaternion.LookRotation(player.forward)));
        }
    }
    IEnumerator CamLockedOnTarget(Transform targetEnemy)
    {
        while (isLocked)
        {
            if (isSmoothLooking)
            {
                yield return null;
                continue;
            }
            camFollowTarget.LookAt(new Vector3(targetEnemy.position.x, targetEnemy.position.y - yAxisLockOffset, targetEnemy.position.z));
            if(Vector3.Distance(camFollowTarget.position, targetEnemy.position) > viewRadius * maxLockOnDistance)
            {
                isLocked = false;
                enemyHealthbar.Hide();
            }
            yield return null;
        }
    }
    // This function was inspired by Sebastian Lague
    // https://www.youtube.com/watch?v=rQG9aUWarwE&ab_channel=SebastianLague
    void FindEnemiesInSight()
    {
        enemiesInSight.Clear();
        Collider[] enemiesInViewRadius = Physics.OverlapSphere(camFollowTarget.position, viewRadius, lockableMask);
        for (int i = 0; i < enemiesInViewRadius.Length; i++)
        {
            Transform enemy = enemiesInViewRadius[i].transform;
            Vector3 directionToEnemy = (enemy.position - Camera.main.transform.position).normalized;
            if (Vector3.Angle(Camera.main.transform.forward, directionToEnemy) < viewAngle / 2)
            {
                float distanceToTarget = Vector3.Distance(Camera.main.transform.position, enemy.position);
                if (!Physics.Raycast(Camera.main.transform.position, directionToEnemy, distanceToTarget, environmentMask))
                {
                   //Debug.DrawRay(Camera.main.transform.position, directionToEnemy * distanceToTarget, Color.magenta, 1);
                   enemiesInSight.Add(enemy);
                }
            }
        }
    }
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
    //void OnDrawGizmos()
    //{
    //    Gizmos.color = Color.red;
    //    Gizmos.DrawWireSphere(camFollowTarget.position, viewRadius);
    //    Quaternion leftRayRotation = Quaternion.AngleAxis(-viewAngle / 2, Vector3.up);
    //    Quaternion rightRayRotation = Quaternion.AngleAxis(viewAngle / 2, Vector3.up);
    //    Vector3 leftRayDirection = leftRayRotation * camFollowTarget.forward;
    //    Vector3 rightRayDirection = rightRayRotation * camFollowTarget.forward;
    //    Gizmos.color = Color.green;
    //    Gizmos.DrawRay(Camera.main.transform.position, leftRayDirection * viewRadius);
    //    Gizmos.DrawRay(Camera.main.transform.position, rightRayDirection * viewRadius);

    //}
}
