using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public enum Directions { Left, Right }

public class LockOn : MonoBehaviour
{
    [SerializeField, Range(0, 360)] float viewRadius = 90;
    [SerializeField] LayerMask lockableMask;
    [SerializeField] LayerMask environmentMask;
    [SerializeField] float detectionRefresh = 0.2f;
    [SerializeField] float yAxisLockOffset = 1.5f;
    [SerializeField, Range(1, 2)] float maxLockOnDistance;
    [SerializeField, Range(0, 5)] float healthbarLingerTimeOnEnemyDeath = 1.5f;
    [SerializeField] float mouseThreshold = 80;
    readonly List<Transform> enemiesInSight = new();
    Transform player;
    Transform camFollowTarget;
    bool isSmoothLooking = false;
    InputInterface inputInterface;
    Camera mainCam;
    CameraMovement cameraMovement;
    Coroutine CamLockOnTargetCoroutine;
    public bool IsLocked { get; private set; } = false;
    public Transform TargetEnemy { get; private set; } = null;
    public EnemyHealth EnemyHealth { get; private set; } = null;
    Canvas canvas;
    [SerializeField] RectTransform indicator;
    [SerializeField] float indicatorYOffset;
    [SerializeField] float minTopDownDistance = .2f;


    public void Awake()
    {
        canvas = FindObjectOfType<DisplayBar>().GetComponentInParent<Canvas>();
        player = GetComponentInChildren<CharacterController>().transform;
        inputInterface = GetComponent<InputInterface>();
        camFollowTarget = GameObject.Find("FollowTarget").transform;
        mainCam = Camera.main;
        indicator = GameObject.FindGameObjectWithTag("Indicator").GetComponent<RectTransform>();
        indicator.gameObject.SetActive(false);
        cameraMovement = GetComponent<CameraMovement>();
    }
    public void Start()
    {
        StartCoroutine(FindEnemiesCoroutine());
        StartCoroutine(SwitchTargetCoroutine());
    }
    public void LateUpdate()
    {
        if (IsLocked && EnemyHealth is not BossHealth)
        {
            if (EnemyHealth.displayHealthbar != null)
            {
                RectTransform parent = (EnemyHealth.displayHealthbar as EnemyHealthbar).rt;
                indicator.position = new Vector2(parent.position.x, parent.position.y + indicatorYOffset);
            }
            else
            {
                EnemyHealth.ShowHealthbar();
                indicator.gameObject.SetActive(true);
            }
        }
    }
    IEnumerator SwitchTargetCoroutine()
    {
        while (true)
        {
            if (inputInterface.KeyboardAndMouseActive)
            {
                yield return new WaitUntil(() => IsLocked && (Mathf.Abs(inputInterface.Look.x) > mouseThreshold || inputInterface.GamepadActive));
            }
            else
            {
                yield return new WaitUntil(() => IsLocked && (Mathf.Abs(inputInterface.Look.x) > 0.5f || inputInterface.KeyboardAndMouseActive));
            }
            bool enemyAvailable = false;
            if (inputInterface.Look.x > 0)
                enemyAvailable = SwitchLockedEnemy(Directions.Left);
            else if (inputInterface.Look.x < 0)
                enemyAvailable = SwitchLockedEnemy(Directions.Right);
            if (!enemyAvailable)
                continue;
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnLockOn()
    {
        if (IsLocked)
            StopLocking();
        else if (enemiesInSight.Count > 0)
        {
            IsLocked = true;
            TargetEnemy = FindClosestEnemy();
            LookAtTarget(false);
        }
        else
            StartCoroutine(SmoothLook(Quaternion.LookRotation(player.forward)));
    }

    public void StopLocking()
    {
        IsLocked = false;

        if (EnemyHealth != null && EnemyHealth is not BossHealth)
            EnemyHealth.HideHealthbar();
        indicator.gameObject.SetActive(false);
        EnemyHealth = null;
    }
    IEnumerator CamLockedOnTarget(Transform targetEnemy)
    {
        while (IsLocked)
        {
            float topDownDistance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(targetEnemy.position.x, targetEnemy.position.z));
            // float heightDifference = Mathf.Abs(player.position.y - targetEnemy.position.y);
            if (EnemyHealth != null && EnemyHealth.IsDead || topDownDistance < minTopDownDistance)
            {
                IsLocked = false;
                indicator.gameObject.SetActive(false);
                yield return new WaitForSeconds(healthbarLingerTimeOnEnemyDeath);
                if (!IsLocked && EnemyHealth is not BossHealth && EnemyHealth != null)
                {
                    EnemyHealth.HideHealthbar();
                }
                yield break;
            }
            if (isSmoothLooking)
            {
                yield return null;
                continue;
            }
            camFollowTarget.LookAt(new Vector3(targetEnemy.position.x, targetEnemy.position.y + yAxisLockOffset, targetEnemy.position.z));
            Vector3 euler = camFollowTarget.eulerAngles;
            euler.z = 0;
            euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
            euler.x = Mathf.Clamp(euler.x, cameraMovement.CamMinClamp, cameraMovement.CameraMaxClamp);
            camFollowTarget.eulerAngles = new Vector3(euler.x, euler.y, 0);

            if (Vector3.Distance(camFollowTarget.position, targetEnemy.position) > viewRadius * maxLockOnDistance)
                StopLocking();
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

            Vector3 enemyPos = mainCam.WorldToViewportPoint(enemy.position);

            float topDownDistance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(enemy.position.x, enemy.position.z));
            if (EnemyInCamAngle(enemyPos) && EnemyInRangeAndSight(directionToEnemy, distanceToTarget) && topDownDistance > minTopDownDistance)
            {
                enemiesInSight.Add(enemy);
                //Debug.DrawRay(Camera.main.transform.position, directionToEnemy * distanceToTarget, Color.magenta, 1);
            }
        }
    }
    // Use Camera.WorldToViewportPoint() for enemyPos
    bool EnemyInCamAngle(Vector3 enemyPos) => enemyPos.x > 0 && enemyPos.x < 1 && enemyPos.y > 0 && enemyPos.y < 1 && enemyPos.z > 0;
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
            Vector3 euler = camFollowTarget.eulerAngles;
            euler.z = 0;
            euler.x = euler.x > 180 ? euler.x - 360 : euler.x;
            euler.x = Mathf.Clamp(euler.x, cameraMovement.CamMinClamp, cameraMovement.CameraMaxClamp);
            camFollowTarget.transform.eulerAngles = new Vector3(euler.x, euler.y, 0);
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
    void LookAtTarget(bool isSwitchingTarget)
    {
        EnemyHealth = TargetEnemy.gameObject.GetComponentInParent<EnemyHealth>();
        if (EnemyHealth != null)
        {
            if (EnemyHealth is not BossHealth) EnemyHealth.ShowHealthbar();
            indicator.gameObject.SetActive(true);
        }

        //Debug.DrawLine(camFollowTarget.position, TargetEnemy.position,Color.blue,1);
        StartCoroutine(SmoothLook(Quaternion.LookRotation(new Vector3(TargetEnemy.position.x, TargetEnemy.position.y - yAxisLockOffset, TargetEnemy.position.z) - camFollowTarget.position)));
        if(isSwitchingTarget)
            StopCoroutine(CamLockOnTargetCoroutine);
        CamLockOnTargetCoroutine = StartCoroutine(CamLockedOnTarget(TargetEnemy));
    }
    bool SwitchLockedEnemy(Directions dir)
    {
        if (enemiesInSight.Count > 0 && TargetEnemy != null)
        {
            if (enemiesInSight == null)
                return false;

            indicator.gameObject.SetActive(false);
            if (EnemyHealth is not BossHealth)
                EnemyHealth.HideHealthbar();
            List<Transform> enemies = enemiesInSight.OrderBy((e) => mainCam.WorldToViewportPoint(e.position).x).ToList();

            int targetIndex = enemies.FindIndex((e) => mainCam.WorldToViewportPoint(TargetEnemy.position).x == mainCam.WorldToViewportPoint(e.position).x);
            if (dir == Directions.Right)
                targetIndex--;
            else if (dir == Directions.Left)
                targetIndex++;
            Debug.Log(targetIndex);
            if (targetIndex >= 0 && targetIndex < enemies.Count)
                TargetEnemy = enemies[targetIndex];
            else
            {
                indicator.gameObject.SetActive(true);
                return false;
            }
            LookAtTarget(true);
            return true;
        }
        return false;
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
