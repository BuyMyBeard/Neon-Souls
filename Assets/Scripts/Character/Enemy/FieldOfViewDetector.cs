using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine.AI;
using System.Linq;

[RequireComponent(typeof(Enemy))]
public class FieldOfViewDetector : MonoBehaviour, IPlayerDetector
{
    const float TimeBetweenChecks = .2f;
    [SerializeField] Transform eyes;
    [SerializeField] LayerMask environmentMask;


    //why is viewRange not used anywhere?
    [SerializeField] float viewRange = 40f;
    
    [Range(0f, 90f)]
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float timeToDetect = .6f;
    [SerializeField] float timeToForget = 10;
    [SerializeField] bool ignoreIfUnreachable = true;
    Transform playerTarget;
    Enemy enemy;
    NavMeshAgent agent;
    float timeInSight = 0;
    public float DotViewAngle { get => math.remap(0, 180, 1, 0, viewAngle); }
    private void Start()
    {
        enemy = GetComponent<Enemy>();
        playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        enemy.idleInitEvent.AddListener(StartDetectingPlayer);
        enemy.idleExitEvent.AddListener(StopDetectingPlayer);
        agent = GetComponent<NavMeshAgent>();
    }
    bool IsPlayerSighted
    {
        get
        {
            float distanceToTarget = Vector3.Distance(eyes.position, playerTarget.position);
            Vector3 directionToTarget = playerTarget.position - eyes.position;

            return (IsInViewAngle(directionToTarget) && TargetInRangeAndSight(directionToTarget, distanceToTarget));
        }
    }
    bool CanReachTarget
    {
        get
        {
            NavMeshPath path = new NavMeshPath();
            return agent.CalculatePath(playerTarget.position, path) && path.status == NavMeshPathStatus.PathComplete;
        }
    }
    bool IsInViewAngle(Vector3 directionToTarget)
    {
        Vector2 flatDelta = new(directionToTarget.x, directionToTarget.z);
        Vector2 flatEnemyForward = new(eyes.forward.x, eyes.forward.z);
        float dot = Vector2.Dot(flatEnemyForward.normalized, flatDelta.normalized);
        return dot > DotViewAngle;
    }
    bool TargetInRangeAndSight(Vector3 directionToTarget, float distanceToTarget) => distanceToTarget < viewRange && !Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, environmentMask);

    void StartDetectingPlayer()
    {
        StartCoroutine(nameof(DetectPlayer));
        StopCoroutine(nameof(ForgetPlayer));
    }
    void StopDetectingPlayer()
    {
        StopCoroutine(nameof(DetectPlayer));
        StartCoroutine(nameof(ForgetPlayer));
    }
    IEnumerator DetectPlayer()
    {
        while (true)
        {
            yield return new WaitForSeconds(TimeBetweenChecks);
            if (IsPlayerSighted && (!ignoreIfUnreachable || CanReachTarget))
            {
                timeInSight += TimeBetweenChecks;
                if (timeInSight >= timeToDetect)
                    enemy.ChangeMode(Enemy.ModeId.InRange);
            }
            else
                timeInSight = 0;
        }
    }

    IEnumerator ForgetPlayer()
    {
        const float CheckDelay = .5f;
        float forgetTimer = 0;
        while (forgetTimer < timeToForget)
        {
            yield return new WaitForSeconds(CheckDelay);
            if (IsPlayerSighted) forgetTimer = 0;
            else forgetTimer += CheckDelay;
        }
        enemy.ChangeMode(Enemy.ModeId.Idle);
    }
}
