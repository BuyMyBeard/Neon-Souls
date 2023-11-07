using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMeleeAttack : MeleeAttack ,IStat
{
    Stamina stamina;
    PlayerAnimationEvents animationEvents;
    [SerializeField] int dmgUpgrade = 0;
    public float Value => baseDamage;
    public int Upgrade => dmgUpgrade;
    protected override void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    public override void InitWeaponCollider(AttackDef attackDef)
    {
        base.InitWeaponCollider(attackDef);
        stamina.Remove(attackDef.staminaCost);
    }
    void OnLightAttack()
    {
        if (!animationEvents.ActionAvailable || stamina.IsExhausted) return;
        
        animator.SetTrigger("LightAttack");
        animationEvents.StopStaminaRegen();
        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
    }
    void OnHeavyAttack()
    {
        if (!animationEvents.ActionAvailable || stamina.IsExhausted) return;

        animator.SetTrigger("HeavyAttack");
        animationEvents.StopStaminaRegen();
        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
    }
    public void UpgradeStat(int nbAmelioration)
    {
        bonusDamage += nbAmelioration * dmgUpgrade;
    }
}