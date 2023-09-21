using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerAttack : MonoBehaviour
{
    [SerializeField] int lightAttackStaminaCost = 15;
    [SerializeField] int heavyAttackStaminaCost = 30;
    [SerializeField] int lightAttackDamage = 25;
    [SerializeField] int heavyAttackDamage = 50;

    Sword sword;
    Animator animator;
    Stamina stamina;

    readonly List<Health> enemiesHit = new();
    public void EnableSwordCollider()
    {
        sword.ColliderEnabled = true;
    }
    public void DisableSwordCollider()
    {
        sword.ColliderEnabled = false;
        enemiesHit.Clear();
    }
    void Awake()
    {
        stamina = GetComponent<Stamina>();
        animator = GetComponent<Animator>();       
        sword = GetComponentInChildren<Sword>();

        if (sword == null)
            throw new MissingComponentException("Sword component missing on player");
        else
        {
            sword.onTrigger.AddListener(OnAttackHit);
            DisableSwordCollider();
        }
    }

    void OnLightAttack()
    {
        if (!stamina.IsExhausted)
        {
            stamina.Remove(lightAttackStaminaCost);
            animator.SetTrigger("LightAttack");
        }
    }

    void OnHeavyAttack()
    {

    }
    void OnAttackHit(Collider other)
    {
        Health enemyHealth = other.GetComponentInParent<Health>();
        if (enemyHealth == null)
            throw new MissingComponentException("Enemy has missing Health component");
        // To avoid hitting the same enemy multiple times with the same attack
        else if (!enemiesHit.Contains(enemyHealth))
        {
            enemiesHit.Add(enemyHealth);
            enemyHealth.TakeDamage(15);
        }
    }
}
