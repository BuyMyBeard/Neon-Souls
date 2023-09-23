using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour, IRechargeable
{
    GameManager gameManager;
    DisplayBar playerHealthbar;
    [SerializeField] float maxHealth = 100;
    float currentHealth;

    public float CurrentHealth { get => currentHealth; }
    void Awake()
    {
        playerHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();
        gameManager =FindObjectOfType<GameManager>();
    }
    private void OnEnable()
    {
        ResetHealth();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        playerHealthbar.Remove(damage, maxHealth, true);//TODO: change to false.
        if(currentHealth <= 0) 
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        gameManager.PlayerDie();
    }

    public void Heal(float healthRestored)
    {
        currentHealth += healthRestored;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        playerHealthbar.Add(healthRestored, maxHealth);
        
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
        playerHealthbar.Add(maxHealth, maxHealth);
    }
    void OnLightAttack()
    {
        TakeDamage(10);
    }
    void OnHeavyAttack()
    {
        TakeDamage(20);
    }
    public void Round() => currentHealth = Mathf.RoundToInt(currentHealth);

    public void Recharge()
    {
        ResetHealth();
    }
}
