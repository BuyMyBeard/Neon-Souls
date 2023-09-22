using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
public abstract class MeleeAttack : MonoBehaviour
{

    protected MeleeWeapon weapon;
    protected Animator animator;

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
        animator = GetComponent<Animator>();       
        weapon = GetComponentInChildren<MeleeWeapon>();
    }
    protected virtual void Start()
    {
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
