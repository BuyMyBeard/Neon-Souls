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
    [SerializeField] int ameliorateur = 0;

    public float Value => ameliorateur;
    public int Upgrade => ameliorateur;
    protected override void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    public override void InitWeaponCollider(AttackDef attackDef, int modifAtk = 0)
    {
        base.InitWeaponCollider(attackDef,Upgrade);
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
       /* lightAttackDamage += ameliorateur * nbAmelioration;
        heavyAttackDamage += ameliorateur * nbAmelioration;*/
    }
}