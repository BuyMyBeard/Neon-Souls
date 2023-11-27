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
    public Quaternion OriginRot { get; private set; }
    public Vector3 OriginPos { get; private set; }
    protected EnemyAnimationEvents enemyAnimationEvents;
    public bool canRespawn = true;
    [SerializeField] float baseTurnSpeed;
    [SerializeField] int xpPrice = 5;
    [HideInInspector] public float turnSpeed;
    public bool rotationFrozen = false;
    public bool movementFrozen = false;
    public float BaseSpeed { get; protected set; }


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
    public Vector3 Velocity { get; private set; }
    protected Vector3 prevPosition;
    [SerializeField] Collider lockable;
    LockOn lockOn;
    XpManager xpManager;
    EnemyHealth health;
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
        
        turnSpeed = baseTurnSpeed;
        BaseSpeed = agent.speed;
        prevPosition = transform.position;
        Mode.Init();
        xpManager = FindObjectOfType<XpManager>();
        OriginPos = transform.position;
        OriginRot = transform.rotation;
        lockOn = Target.GetComponentInParent<LockOn>();
        health = GetComponent<EnemyHealth>();
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
        DirectionToPlayer = target - current;
        distanceFromPlayer = Vector3.Distance(target, current);
        Velocity = (transform.position - prevPosition) / Time.deltaTime;
        prevPosition = transform.position;
        AnimateMovement();
    }

    public void ChangeMode(ModeId modeId)
    {
        if (lockMode || !gameObject.activeInHierarchy) return;
        // if (Mode.Id == modeId) return;
        Mode.Exit();
        Mode = modeDefs[(int)modeId];
        Mode.Init();
    }
    protected virtual void IdleInit() => idleInitEvent.Invoke();
    protected virtual void InRangeInit() => inRangeInitEvent.Invoke();
    protected virtual void CloseInit() => closeInitEvent.Invoke();
    protected virtual void IdleMain() { }
    protected virtual void InRangeMain() { }
    protected virtual void CloseMain()
    {
        if (!rotationFrozen)
        {
            Quaternion towardsPlayer = Quaternion.LookRotation(DirectionToPlayer, Vector3.up);
            transform.rotation = Quaternion.RotateTowards(transform.rotation, towardsPlayer, turnSpeed * Time.deltaTime);
        }
    }
    protected virtual void IdleExit() => idleExitEvent.Invoke();
    protected virtual void InRangeExit() => inRangeExitEvent.Invoke();
    protected virtual void CloseExit() => closeExitEvent.Invoke();

    public virtual void Recharge(RechargeType rechargeType)
    {
        if (!canRespawn && health.hasAlreadyDiedOnce) return;
        gameObject.SetActive(false);
        transform.SetPositionAndRotation(OriginPos, OriginRot);
        gameObject.SetActive(true);
        animator.SetBool("IsMoving", false);
        animator.SetBool("IsRunning", false);
        animator.Play("Idle");
        enemyAnimationEvents.ResetAll();
        ChangeMode(ModeId.Idle);

    }
    public void RestoreTurnSpeed() => turnSpeed = baseTurnSpeed;
    public void RestoreSpeed() => agent.speed = BaseSpeed;
    protected void AnimateMovement()
    {
        if (!enemyAnimationEvents.ActionAvailable || movementFrozen)
        {
            animator.SetBool("IsMoving", false);
            return;
        }
        animator.SetBool("IsMoving", Velocity.magnitude > 0);
        Vector3 relativeVelocity = transform.InverseTransformDirection(Velocity);
        Vector2 flatRelativeVelocity = new Vector2(relativeVelocity.x, relativeVelocity.z).normalized;
        animator.SetFloat("MovementX", flatRelativeVelocity.x);
        animator.SetFloat("MovementY", flatRelativeVelocity.y);
    }
    public void GiveXp()
    {
        if (xpPrice <= 0) return; 
        xpManager.DistributeXp(xpPrice);
    }

    public void DisableLockOn()
    {
        lockable.enabled = false;
        lockOn.StopLocking();
    }

    public void EnableLockOn()
    {
        lockable.enabled = true;
    }
}