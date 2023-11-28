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
    //protected Dictionary<AttackWeapon, MeleeWeapon> weaponColliders = new();
    protected Animator animator;
    public bool isAttacking = false;
    public int baseDamage;
    public int bonusDamage = 0;
    protected Health health;

    /*private void OnValidate()
    {
        weaponColliders.Clear();
        foreach (var e in weaponCollidersStructs)
        {
            weaponColliders.Add(e.key, e.val);
        }
    }*/
    /// <summary>
    /// Enables the weapon's collider and initializes the damage
    /// </summary>
    /// <param name="attackDef"></param>
    public virtual void InitWeaponCollider(AttackDef attackDef)
    {
        MeleeWeapon mw = GetMeleeWeaponFromAttackDef(attackDef);//weaponColliders[attackDef.weapon];
        mw.ColliderEnabled = true;
        mw.damage = Mathf.FloorToInt(baseDamage * attackDef.baseDamageMultiplier) + bonusDamage;
        mw.staminaBlockCost = attackDef.staminaCost;
        mw.blockable = attackDef.blockable;
    }

    /// <summary>
    /// Disables the children weapon collider
    /// </summary>
    public void DisableWeaponCollider(AttackDef attackDef)
    {
        GetMeleeWeaponFromAttackDef(attackDef).ColliderEnabled = false;
    }
    public void DisableAllWeaponColliders()
    {
        foreach (var meleeWeapon in weaponCollidersStructs)
        {
            meleeWeapon.val.ColliderEnabled = false;
            //meleeWeapon.ColliderEnabled = false;
        }
        if (this is EnemyMeleeAttack) (this as EnemyMeleeAttack).StopFlickerBodyCollider();
    }
    protected virtual void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        health = GetComponent<Health>();
    }
    protected virtual void Start()
    {
        if (animator == null)
            throw new MissingComponentException("Animator component missing on character");

        DisableAllWeaponColliders();
    }
    protected MeleeWeapon GetMeleeWeaponFromAttackDef(AttackDef attackDef) => Array.Find(weaponCollidersStructs, (weapon) => weapon.key == attackDef.weapon).val;
    
}
