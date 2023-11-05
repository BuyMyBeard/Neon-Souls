using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Enemy : MonoBehaviour,IXpGiver
{
    [SerializeField] int xpPrice = 5;
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

    [SerializeField] protected Transform target;
    public Transform Target => target;
    private Vector3 distanceFromPlayer;
    public Vector3 DistanceFromPlayer => distanceFromPlayer;

    ModeDef[] modeDefs;
    protected ModeDef Mode { get; private set; }
    protected bool lockMode = false;
    XpManager xpManager;
    protected virtual void Awake()
    {
        modeDefs = new ModeDef[3]
        {
            new ModeDef { Id = ModeId.Idle, Init = IdleInit, Main = IdleMain, Exit = IdleExit },
            new ModeDef { Id = ModeId.InRange, Init = InRangeInit, Main = InRangeMain, Exit = InRangeExit },
            new ModeDef { Id = ModeId.Close, Init = CloseInit, Main = CloseMain, Exit = CloseExit },
        };
        Mode = modeDefs[(int)ModeId.Idle];
        Mode.Init();
        xpManager = FindObjectOfType<XpManager>();
    }
    void Update()
    {
        distanceFromPlayer = transform.position - target.position;
        distanceFromPlayer.y = 0;
        Mode.Main();
    }

    public void ChangeMode(ModeId modeId)
    {
        if (lockMode) return;
        if (Mode.Id == modeId) return;
        //Debug.Log($"Enemy.ChangeMode(): {GetInstanceID()} {Enum.GetName(typeof(ModeId), Mode.Id)} -> {Enum.GetName(typeof(ModeId), modeId)}");
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

    public void GiveXp()
    {
        xpManager.DistributeXp(xpPrice);
    }
}