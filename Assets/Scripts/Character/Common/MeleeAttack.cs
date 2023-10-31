using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MeleeAttack : MonoBehaviour
{
    [Serializable]
    public struct WeaponEnumCollider
    {
        public AttackWeapon key;
        public MeleeWeapon val;
    }
    [SerializeField] protected WeaponEnumCollider[] weaponCollidersStructs;
    protected Dictionary<AttackWeapon, MeleeWeapon> weaponColliders = new();
    protected Animator animator;
    public bool isAttacking = false;
    public int baseDamage;
    public int damageBonus = 0;

    private void OnValidate()
    {
        weaponColliders.Clear();
        foreach (var e in weaponCollidersStructs)
        {
            weaponColliders.Add(e.key, e.val);
        }
    }
    /// <summary>
    /// Enables the weapon's collider and initializes the damage
    /// </summary>
    /// <param name="attackDef"></param>
    public virtual void InitWeaponCollider(AttackDef attackDef)
    {
        MeleeWeapon mw = weaponColliders[attackDef.weapon];
        mw.ColliderEnabled = true;
        mw.damage = Mathf.FloorToInt(baseDamage * attackDef.baseDamageMultiplier) + damageBonus;
    }
    /// <summary>
    /// Disables the children weapon collider
    /// </summary>
    public void DisableWeaponCollider(AttackDef attackDef)
    {
        weaponColliders[attackDef.weapon].ColliderEnabled = false;
    }
    public void DisableAllWeaponColliders()
    {
        foreach (var meleeWeapon in weaponColliders.Values)
        {
            meleeWeapon.ColliderEnabled = false;
        }
    }
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
    }
    protected virtual void Start()
    {
        if (animator == null)
            throw new MissingComponentException("Animator component missing on character");

        DisableAllWeaponColliders();
    }
}
