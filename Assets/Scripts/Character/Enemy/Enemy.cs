using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
public abstract class Enemy : MonoBehaviour
{
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
    private Vector3 distanceFromPlayer;
    public Vector3 DistanceFromPlayer => distanceFromPlayer;

    ModeDef[] modeDefs;
    protected ModeDef Mode { get; private set; }
    protected bool lockMode = false;
    protected NavMeshAgent agent;
    protected Animator animator;

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
    }
    protected virtual void OnEnable()
    {
        Mode.Init();
    }
    protected virtual void Update()
    {
        distanceFromPlayer = transform.position - Target.position;
        distanceFromPlayer.y = 0;
        Mode.Main();
    }

    public void ChangeMode(ModeId modeId)
    {
        if (lockMode) return;
        if (Mode.Id == modeId) return;
        Mode.Exit();
        Mode = modeDefs[(int)modeId];
        Mode.Init();
    }
    protected virtual void IdleInit() { }
    protected virtual void InRangeInit() { }
    protected virtual void CloseInit() { }
    protected virtual void IdleMain() { }
    protected virtual void InRangeMain() { }
    protected virtual void CloseMain() { }
    protected virtual void IdleExit() { }
    protected virtual void InRangeExit() { }
    protected virtual void CloseExit() { }
}