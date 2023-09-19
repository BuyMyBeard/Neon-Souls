using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    DisplayBar playerHealthbar;
    [SerializeField] int maxHealth = 100;
    [SerializeField] int currentHealth;

    void Awake()
    {
        playerHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();    
    }

    void Update()
    {
        
    }
    private void OnEnable()
    {
        currentHealth = maxHealth;
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
        throw new NotImplementedException();
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
