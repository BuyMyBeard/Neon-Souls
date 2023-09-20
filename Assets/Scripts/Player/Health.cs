using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    GameManager gameManager;
    DisplayBar playerHealthbar;
    RespawnManager respawnManager;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    void Awake()
    {
        playerHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();    
        gameManager = FindObjectOfType<GameManager>();
    }

    void Update()
    {
        
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
            Die();
        }
    }

    private void Die()
    {
        gameManager.PlayerDie();
        Heal(100);
    }

    public void Heal(int healthRestored)
    {
        currentHealth += healthRestored;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        playerHealthbar.Add(healthRestored, maxHealth);
        
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    void OnLightAttack()
    {
        TakeDamage(10);
    }
    void OnHeavyAttack()
    {
        TakeDamage(20);
    }

}
