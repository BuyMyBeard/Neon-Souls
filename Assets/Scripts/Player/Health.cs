using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour
{
    public DisplayBar displayHealthbar;
    [SerializeField] float maxHealth = 100;
    float currentHealth;
    [SerializeField] string healthbarTag = "PlayerHealthbar";

    public float CurrentHealth { get => currentHealth; }
    public float MaxHealth { get => maxHealth; }
    void Awake()
    {

        displayHealthbar = GameObject.FindGameObjectWithTag(healthbarTag).GetComponent<DisplayBar>();    
    }
    private void OnEnable()
    {
        ResetHealth();
    }
    public void TakeDamage(int damage)
    {
        currentHealth -= damage;
        if(displayHealthbar != null)
            displayHealthbar.Remove(damage, maxHealth, true);//TODO: change to false.
        if(currentHealth <= 0) 
        {
            currentHealth = 0;
            Die();
        }
    }

    private void Die()
    {
        throw new NotImplementedException();
    }

    public void Heal(float healthRestored)
    {
        currentHealth += healthRestored;
        if (currentHealth > maxHealth)
            currentHealth = maxHealth;
        displayHealthbar.Add(healthRestored, maxHealth);
        
    }

    private void ResetHealth()
    {
        currentHealth = maxHealth;
    }
    public void Round() => currentHealth = Mathf.RoundToInt(currentHealth);
}
