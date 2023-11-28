using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Health : MonoBehaviour, IRechargeable
{
    [SerializeField] protected float maxHealth = 100;
    [SerializeField] protected float timeShowingHealthbar = 3;
    public DisplayBar displayHealthbar;
    public bool invincible = false;
    protected Animator animator;
    public UnityEvent OnHit;
    [HideInInspector] public UnityEvent<Health> OnHitWithParam;
    public float CurrentHealth { get; protected set; }
    public bool IsDead { get => CurrentHealth <= 0; }
    public float MaxHealth { get => maxHealth; }
    protected AnimationEvents animationEvents;
    protected LockOn lockOn;
    protected FallApart fallApart;
    protected bool canFallApart;
    protected bool staggerable;
    protected Stagger stagger;
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<AnimationEvents>();
        lockOn = FindObjectOfType<LockOn>();   
        canFallApart = TryGetComponent(out fallApart);
        staggerable = TryGetComponent(out stagger);
    }
    void OnEnable()
    {
        ResetHealth();
    }

    public abstract void HandleHealthbar(int damage);


    //TODO: Refactor this to make it not public
    /// <summary>
    /// Apply damage to health
    /// </summary>
    /// <param name="damage">Amount of damage applied</param>
    public void InflictDamage(int damage, Transform attackerPosition = null)
    {
        if (invincible && IsDead)
            return;
        if (attackerPosition != null)
            stagger.BecomeStaggered(attackerPosition);
        OnHit.Invoke();
        OnHitWithParam.Invoke(this);
        CurrentHealth -= damage;
        HandleHealthbar(damage);
        if (IsDead)
        {
            CurrentHealth = 0;
            Die();
        }
    }
    public virtual void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition, BlockSound blockSound = BlockSound.SwordClash)
    {
        // One day enemies might be able to block or have stamina idk
        // for now it's just
        InflictDamage(damage);
    }
    protected virtual void Die()
    {
        invincible = true;
        animator.ResetAllTriggers();
        animator.SetTrigger("Die");
        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
        animationEvents.StartIFrame();
        if (canFallApart) fallApart.DetachWeapons();
    }

    /// <summary>
    /// Restores health
    /// </summary>
    /// <param name="healthRestored">Amount of health restored</param>
    public void Heal(float healthRestored)
    {
        if (IsDead) return;
        CurrentHealth += healthRestored;
        if (CurrentHealth > maxHealth)
            CurrentHealth = maxHealth;
        displayHealthbar.Add(healthRestored, maxHealth);
    }
    /// <summary>
    /// Returns current health back to full
    /// </summary>
    protected void ResetHealth()
    {
        CurrentHealth = maxHealth;
    }
    /// <summary>
    /// Rounds current health to the nearest integer. Used to avoid float imprecision caused by healing over time
    /// </summary>
    public void Round() => CurrentHealth = Mathf.RoundToInt(CurrentHealth);

    public virtual void Recharge(RechargeType rechargeType)
    {
        ResetHealth();
        invincible = false;
    }
}
