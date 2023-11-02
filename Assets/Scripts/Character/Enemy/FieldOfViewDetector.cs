using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.XR;
using Unity.Mathematics;

public class FieldOfViewDetector : MonoBehaviour, IPlayerDetector
{
    [SerializeField] Transform eyes;
    [SerializeField] LayerMask environmentMask;
    [SerializeField] float viewRange = 40f;
    [Range(0f, 90f)]
    [SerializeField] float viewAngle = 90f;
    Transform playerTarget;
    Enemy enemy;
    public float DotViewAngle { get => math.remap(0, 180, 1, 0, viewAngle); }
    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        playerTarget = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
    }
    private void Start()
    {
        enemy.idleInitEvent.AddListener(StartDetectingPlayer);
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
        return dot < DotViewAngle;
    }
    bool TargetInRangeAndSight(Vector3 directionToTarget, float distanceToTarget) => !Physics.Raycast(eyes.position, directionToTarget, distanceToTarget, environmentMask);

    void StartDetectingPlayer() => StartCoroutine(DetectPlayer());
    IEnumerator DetectPlayer()
    {
        yield return new WaitUntil(() => IsPlayerSighted);
        enemy.ChangeMode(Enemy.ModeId.InRange);
    }
}
