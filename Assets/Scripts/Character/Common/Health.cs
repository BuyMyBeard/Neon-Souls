using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour,IRechargeable
{
    [SerializeField] float maxHealth = 100;
    [SerializeField] string healthbarTag = "PlayerHealthbar";
    [SerializeField] bool debugDieOnHeal = false;
    float currentHealth;
    DisplayBar healthbar;
    GameManager manager;
    void Awake()
    {
        healthbar = GameObject.FindGameObjectWithTag(healthbarTag).GetComponent<DisplayBar>();    
        manager = FindObjectOfType<GameManager>();
    }
    private void OnEnable()
    {
        ResetHealth();
    }
    /// <summary>
    /// Apply damage to health
    /// </summary>
    /// <param name="damage">Amount of damage applied</param>
    public void InflictDamage(int damage)
    {
        currentHealth -= damage;
        healthbar.Remove(damage, maxHealth, true);//TODO: change to false.
        if(currentHealth <= 0) 
        {
            currentHealth = 0;
            Die();
        }
    }
    private void Die()
    {
        manager.PlayerDie();
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
        healthbar.Add(healthRestored, maxHealth);
        if (debugDieOnHeal)
            Die();
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
        healthbar.Add(maxHealth, maxHealth);
    }
}
