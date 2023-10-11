using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour,IRechargeable
{
    public DisplayBar displayHealthbar;
    [SerializeField] float maxHealth = 100;
    [SerializeField] string healthbarTag = "PlayerHealthbar";
    public bool invincible = false;
    PlayerAnimationEvents animationEvents;
    GameManager manager;

    float currentHealth;
    Animator animator;
    public float CurrentHealth { get => currentHealth; }

    public bool IsDead { get => currentHealth <= 0; }
    public float MaxHealth { get => maxHealth; }
    void Awake()
    {
        if (displayHealthbar == null)
            displayHealthbar = GameObject.FindGameObjectWithTag(healthbarTag).GetComponent<DisplayBar>();    
        animator = GetComponentInChildren<Animator>();
        manager = FindObjectOfType<GameManager>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    void OnEnable()
    {
        ResetHealth();
    }
    /// <summary>
    /// Apply damage to health
    /// </summary>
    /// <param name="damage">Amount of damage applied</param>
    public void InflictDamage(int damage)
    {
        if (invincible)
            return;
        currentHealth -= damage;
        if(displayHealthbar != null)
            displayHealthbar.Remove(damage, maxHealth, true);//TODO: change to false.
        if(IsDead) 
        {
            currentHealth = 0;
            Die();
        }
    }
    private void Die()
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
            throw new NotImplementedException("Enemy death has not been implemented yet");
    }


    /// <summary>
    /// Restores health
    /// </summary>
    /// <param name="healthRestored">Amount of health restored</param>
    public void Heal(float healthRestored)
    {
        currentHealth += healthRestored;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        displayHealthbar.Add(healthRestored, maxHealth);
    }
    /// <summary>
    /// Returns current health back to full
    /// </summary>
    private void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    /// <summary>
    /// Rounds current health to the nearest integer. Used to avoid float imprecision caused by healing over time
    /// </summary>
    public void Round() => currentHealth = Mathf.RoundToInt(currentHealth);
    
    public void Recharge()
    {
        ResetHealth();
        displayHealthbar.Add(maxHealth, maxHealth);
        if (CompareTag("Player"))
        {
            animationEvents.EnableActions();
            animationEvents.UnFreezeMovement();
            animationEvents.UnFreezeRotation();
            animationEvents.StopIFrame();
            animator.Play("Idle");
        }
    }
}
