using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour
{
    protected enum ModeId
    {
        Idle,
        InRange,
        Close
    }
    protected struct ModeDef
    {
        public ModeId Id { get; init; }
        public Action Init { get; init; }
        public Action Main { get; init; }
        public Action Exit { get; init; }
    }

    [SerializeField] protected Transform target;

    [SerializeField] float moveTowardsThreshold;
    [SerializeField] float noticeThreshold;
    protected Vector3 posDifference;

    ModeDef[] modeDefs;
    protected ModeDef Mode { get; private set; }
    protected bool lockMode = false;

    protected virtual void Awake()
    {
        modeDefs = new ModeDef[3]
        {
            new ModeDef { Id = ModeId.Idle, Init = IdleInit, Main = IdleMain, Exit = IdleExit },
            new ModeDef { Id = ModeId.InRange, Init = InRangeInit, Main = InRangeMain, Exit = InRangeExit },
            new ModeDef { Id = ModeId.Close, Init = CloseInit, Main = CloseMain, Exit = CloseExit },
        };
        Mode = modeDefs[(int)ModeId.Idle];
    }
    void Update()
    {
        posDifference = transform.position - target.position;
        posDifference.y = 0;

        if (posDifference.magnitude > noticeThreshold)
            ChangeMode(ModeId.Idle);
        else if (posDifference.magnitude > moveTowardsThreshold)
            ChangeMode(ModeId.InRange);
        else
            ChangeMode(ModeId.Close);

        Mode.Main();
    }

    void ChangeMode(ModeId modeId)
    {
        if (lockMode) return;
        if (Mode.Id == modeId) return;
        Debug.Log($"Enemy.ChangeMode(): {Enum.GetName(typeof(ModeId), Mode.Id)} -> {Enum.GetName(typeof(ModeId), modeId)}");
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