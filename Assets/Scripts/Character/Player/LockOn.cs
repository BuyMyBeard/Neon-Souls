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
    EnemyHealthbar enemyHealthbar = null;
    Transform player;
    Transform camFollowTarget;
    bool isSmoothLooking = false;
    PlayerController playerController;
    Camera mainCam;
    CameraMovement cameraMovement;
    Coroutine CamLockOnTargetCoroutine;
    public bool IsLocked { get; private set; } = false;
    public Transform TargetEnemy { get; private set; } = null;
    public EnemyHealth enemyHealth { get; private set; } = null;
    Canvas canvas;
    [SerializeField] RectTransform indicator;
    [SerializeField] float minTopDownDistance = .2f;


    public void Awake()
    {
        canvas = FindObjectOfType<DisplayBar>().GetComponentInParent<Canvas>();
        player = GetComponentInChildren<CharacterController>().transform;
        playerController = GetComponent<PlayerController>();
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
        if (IsLocked)
        {
            if(enemyHealth.displayHealthbar != null)
            {
                RectTransform parent = (enemyHealth.displayHealthbar as EnemyHealthbar).rt;
                var vp2 = mainCam.WorldToViewportPoint(TargetEnemy.position);
                var sp2 = canvas.worldCamera.ViewportToScreenPoint(vp2);
                RectTransformUtility.ScreenPointToWorldPointInRectangle(parent, sp2, canvas.worldCamera, out Vector3 worldPoint2);
                indicator.position = worldPoint2;
            }
            else
            {
                enemyHealth.ShowHealthbar();
                indicator.gameObject.SetActive(true);
            }
        }
    }
    IEnumerator SwitchTargetCoroutine()
    {
        while (true)
        {
            if (playerController.KeyboardAndMouseActive)
            {
                yield return new WaitUntil(() => IsLocked && Mathf.Abs(playerController.Look.x) > mouseThreshold || playerController.GamepadActive);
            }
            else
            {
                yield return new WaitUntil(() => IsLocked && Mathf.Abs(playerController.Look.x) > 0.5f || playerController.KeyboardAndMouseActive);
            }
            bool enemyAvailable = false;
            if (playerController.Look.x > 0)
                enemyAvailable = SwitchLockedEnemy(Directions.Left);
            else if (playerController.Look.x < 0)
                enemyAvailable = SwitchLockedEnemy(Directions.Right);
            if (!enemyAvailable)
                continue;
            yield return new WaitForSeconds(0.3f);
        }
    }

    void OnLockOn()
    {
        if (IsLocked)
        {
            IsLocked = false;
            enemyHealth.HideHealthbar();
            indicator.gameObject.SetActive(false);
            enemyHealth = null;
        }
        else if (enemiesInSight.Count > 0)
        {
            IsLocked = true;
            TargetEnemy = FindClosestEnemy();
            LookAtTarget(false);
        }
        else
            StartCoroutine(SmoothLook(Quaternion.LookRotation(player.forward)));
    }
    IEnumerator CamLockedOnTarget(Transform targetEnemy)
    {
        while (IsLocked)
        {
            float topDownDistance = Vector2.Distance(new Vector2(player.position.x, player.position.z), new Vector2(targetEnemy.position.x, targetEnemy.position.z));
            // float heightDifference = Mathf.Abs(player.position.y - targetEnemy.position.y);
            if (enemyHealth != null && enemyHealth.IsDead || topDownDistance < minTopDownDistance)
            {
                IsLocked = false;
                indicator.gameObject.SetActive(false);
                yield return new WaitForSeconds(healthbarLingerTimeOnEnemyDeath);
                if (!IsLocked)
                {
                    enemyHealth.HideHealthbar();
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
            camFollowTarget.transform.localEulerAngles = new Vector3(euler.x, euler.y, 0);

            if (Vector3.Distance(camFollowTarget.position, targetEnemy.position) > viewRadius * maxLockOnDistance)
            {
                IsLocked = false;
                enemyHealth.HideHealthbar();
                indicator.gameObject.SetActive(false);
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
    bool EnemyInCamAngle(Vector3 enemyPos) => enemyPos.x > 0 && enemyPos.x < 1 && enemyPos.y > 0 && enemyPos.y < 1;
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
            camFollowTarget.transform.localEulerAngles = new Vector3(euler.x, euler.y, 0);
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
        enemyHealth = TargetEnemy.gameObject.GetComponentInParent<EnemyHealth>();
        if (enemyHealth != null)
        {
            enemyHealth.ShowHealthbar();
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
            indicator.gameObject.SetActive(false);
            enemyHealth.HideHealthbar();
            List<Transform> enemies = enemiesInSight.OrderBy((e) => mainCam.WorldToViewportPoint(e.position).x).ToList();

            int targetIndex = enemies.FindIndex((e) => mainCam.WorldToViewportPoint(TargetEnemy.position).x == mainCam.WorldToViewportPoint(e.position).x);
            if (dir == Directions.Right)
                targetIndex--;
            else if (dir == Directions.Left)
                targetIndex++;

            if (targetIndex >= 0 && targetIndex < enemies.Count)
                TargetEnemy = enemies[targetIndex];
            else
                return false;
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
