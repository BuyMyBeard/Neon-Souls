using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerInput))]
public abstract class MeleeAttack : MonoBehaviour
{

    protected MeleeWeapon weapon;
    protected Animator animator;

    readonly List<Health> opponentsHit = new();
    public void EnableWeaponCollider()
    {
        weapon.ColliderEnabled = true;
    }
    public void DisableWeaponCollider()
    {
        weapon.ColliderEnabled = false;
        opponentsHit.Clear();
    }
    protected virtual void Awake()
    {
        animator = GetComponent<Animator>();       
        weapon = GetComponentInChildren<MeleeWeapon>();

        if (weapon == null)
            throw new MissingComponentException("Sword component missing on character");
        else
        {
            weapon.onTrigger.AddListener(OnAttackHit);
            DisableWeaponCollider();
        }
    }

    void OnAttackHit(Collider other)
    {
        Health opponentHealth = other.GetComponentInParent<Health>();
        if (opponentHealth == null)
            throw new MissingComponentException("Character has missing Health component");
        // To avoid hitting the same enemy multiple times with the same attack
        else if (!opponentsHit.Contains(opponentHealth))
        {
            opponentsHit.Add(opponentHealth);
            DamageEnemy(opponentHealth);
        }
    }
    protected abstract void DamageEnemy(Health opponentHealth);
}
