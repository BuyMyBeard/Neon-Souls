using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Unity.Mathematics;
using Unity.VisualScripting;

[RequireComponent(typeof(Enemy))]
public class FieldOfViewDetector : MonoBehaviour, IPlayerDetector
{
    [SerializeField] Transform eyes;
    [SerializeField] LayerMask environmentMask;

    //why is viewRange not used anywhere?
    [SerializeField] float viewRange = 40f;
    
    [Range(0f, 90f)]
    [SerializeField] float viewAngle = 90f;
    [SerializeField] float timeToForget = 10;
    Transform playerTarget;
    Enemy enemy;
    public float DotViewAngle { get => math.remap(0, 180, 1, 0, viewAngle); }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        enemy.idleInitEvent.AddListener(StartDetectingPlayer);
        enemy.idleExitEvent.AddListener(StopDetectingPlayer);
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
        yield return new WaitUntil(() => IsPlayerSighted);
        enemy.ChangeMode(Enemy.ModeId.InRange);
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
