using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Events;
using System.Linq;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour, IRechargeable
{
    Transform origin;
    protected EnemyAnimationEvents enemyAnimationEvents;
    [SerializeField] float baseTurnSpeed;
    [HideInInspector] public float turnSpeed;
    public bool rotationFrozen = false;
    public bool movementFrozen = false;
    public float BaseSpeed { get; protected set; }


    public Vector3 OriginPosition { get => origin.position; }
    public enum ModeId
    {
        Idle,
        InRange,
        Close
    }
    public struct ModeDef
    {
        public ModeId Id { get; init; }
        public Action Init { get; init; }
        public Action Main { get; init; }
        public Action Exit { get; init; }
    }
    public Transform Target { get; protected set; }
    private float distanceFromPlayer;
    public float DistanceFromPlayer => distanceFromPlayer; 
    public Vector3 DirectionToPlayer { get; private set; }
    ModeDef[] modeDefs;
    public ModeDef Mode { get; private set; }
    protected bool lockMode = false;
    protected NavMeshAgent agent;
    protected Animator animator;
    public UnityEvent idleInitEvent = new();
    public UnityEvent inRangeInitEvent = new();
    public UnityEvent closeInitEvent = new();
    public UnityEvent idleExitEvent = new();
    public UnityEvent inRangeExitEvent = new();
    public UnityEvent closeExitEvent = new();

    protected virtual void Awake()
    {
        modeDefs = new ModeDef[3]
        {
            new ModeDef { Id = ModeId.Idle, Init = IdleInit, Main = IdleMain, Exit = IdleExit },
            new ModeDef { Id = ModeId.InRange, Init = InRangeInit, Main = InRangeMain, Exit = InRangeExit },
            new ModeDef { Id = ModeId.Close, Init = CloseInit, Main = CloseMain, Exit = CloseExit },
        };
        Mode = modeDefs[(int)ModeId.Idle];
        Target = GameObject.FindGameObjectWithTag("PlayerTarget").transform;
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        origin = transform;
        turnSpeed = baseTurnSpeed;
        BaseSpeed = agent.speed;
    }
    protected virtual IEnumerator Start()
    {
        yield return null;
        Mode.Init();
    }
    protected virtual void Update()
    {
        Mode.Main();
        Vector3 target = new Vector3(Target.position.x, 0, Target.position.z);
        Vector3 current = new Vector3(transform.position.x, 0, transform.position.z);
        DirectionToPlayer = current - target;
        distanceFromPlayer = Vector3.Distance(target, current);
    }

    public void ChangeMode(ModeId modeId)
    {
        if (lockMode) return;
        if (Mode.Id == modeId) return;
        Mode.Exit();
        Mode = modeDefs[(int)modeId];
        Mode.Init();
        Debug.Log(Mode.Id);
    }
    protected virtual void IdleInit() => idleInitEvent.Invoke();
    protected virtual void InRangeInit() => inRangeInitEvent.Invoke();
    protected virtual void CloseInit() => closeInitEvent.Invoke();
    protected virtual void IdleMain() { }
    protected virtual void InRangeMain() { }
    protected virtual void CloseMain()
    {
        if (rotationFrozen) return;
        Quaternion towardsPlayer = Quaternion.LookRotation(-DirectionToPlayer, Vector3.up);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
    }
    protected virtual void IdleExit() => idleExitEvent.Invoke();
    protected virtual void InRangeExit() => inRangeExitEvent.Invoke();
    protected virtual void CloseExit() => closeExitEvent.Invoke();

    public virtual void Recharge()
    {
        transform.SetPositionAndRotation(origin.position, origin.rotation);
        ChangeMode(ModeId.Idle);
    }
    public void RestoreTurnSpeed() => turnSpeed = baseTurnSpeed;
    public void RestoreSpeed() => agent.speed = BaseSpeed;
}