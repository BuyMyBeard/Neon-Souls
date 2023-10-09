using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public abstract class MeleeAttack : MonoBehaviour
{

    protected MeleeWeapon weapon;
    protected Animator animator;
    public bool isAttacking = false;

    readonly List<Health> opponentsHit = new();
    /// <summary>
    /// Enables the children weapon collider
    /// </summary>
    public void EnableWeaponCollider()
    {
        weapon.ColliderEnabled = true;
    }
    /// <summary>
    /// Disables the children weapon collider
    /// </summary>
    public void DisableWeaponCollider()
    {
        weapon.ColliderEnabled = false;
        opponentsHit.Clear();
    }
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        weapon = GetComponentInChildren<MeleeWeapon>();
    }
    protected virtual void Start()
    {
        if (animator == null)
            throw new MissingComponentException("Animator component missing on character");
        if (weapon == null)
            throw new MissingComponentException("Sword component missing on character");
        else
        {
            weapon.onTrigger.AddListener(OnAttackHit);
            DisableWeaponCollider();
        }
    }
    /// <summary>
    /// Gets called every time the melee weapon hits a collider
    /// </summary>
    /// <param name="other">Other collider hit by the melee weapon</param>
    /// <exception cref="MissingComponentException">Exception thrown if hit recipient doesn't have a Health component</exception>
    void OnAttackHit(Collider other)
    {
        Health opponentHealth = other.GetComponentInParent<Health>();
        if (opponentHealth == null)
            throw new MissingComponentException("Character has missing Health component");
        // To avoid hitting the same enemy multiple times with the same attack
        else if (!opponentsHit.Contains(opponentHealth))
        {
            opponentsHit.Add(opponentHealth);
            DamageOpponent(opponentHealth);
        }
    }
    /// <summary>
    /// Called when hit is successful. Add logic to inflict damage to enemy depending on attack type
    /// </summary>
    /// <param name="opponentHealth">Health component of enemy hit by weapon. Call opponentHealth.InflictDamage() to inflict damage to him</param>
    protected abstract void DamageOpponent(Health opponentHealth);
}
