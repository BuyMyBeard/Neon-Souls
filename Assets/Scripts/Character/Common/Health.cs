using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public DisplayBar displayHealthbar;
    [SerializeField] protected float maxHealth = 100;
    protected string healthbarTag = "EnemyHealthbar";
    public bool invincible = false;
    GameManager manager;
    protected Animator animator;
    public float CurrentHealth { get; protected set; }
    public bool IsDead { get => CurrentHealth <= 0; }
    public float MaxHealth { get => maxHealth; }
    protected PlayerAnimationEvents animationEvents;
    protected void Awake()
    {
        displayHealthbar = GameObject.FindGameObjectWithTag(healthbarTag).GetComponent<DisplayBar>();
        animator = GetComponentInChildren<Animator>();
        manager = FindObjectOfType<GameManager>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    void OnEnable()
    {
        ResetHealth();
    }


    //TODO: Refactor this to make it not public
    /// <summary>
    /// Apply damage to health
    /// </summary>
    /// <param name="damage">Amount of damage applied</param>
    public void InflictDamage(int damage)
    {
        if (invincible)
            return;
        CurrentHealth -= damage;
        if (displayHealthbar != null)
            displayHealthbar.Remove(damage, maxHealth, true);//TODO: change to false.
        if (IsDead)
        {
            CurrentHealth = 0;
            Die();
        }
    }
    public virtual void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition)
    {
        // One day enemies might be able to block or have stamina idk
        // for now it's just
        InflictDamage(damage);
    }

    [ContextMenu("Die")]
    protected virtual void Die()
    {
        if (gameObject.CompareTag("Player"))
        {
            animator.SetTrigger("Die");
            manager.PlayerDie();
            animationEvents.DisableActions();
            animationEvents.FreezeMovement();
            animationEvents.FreezeRotation();
            animationEvents.StartIFrame();
        }
        else
            Debug.Log("EnemyDead");
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
}
