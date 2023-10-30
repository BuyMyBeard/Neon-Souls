using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMeleeAttack : MeleeAttack
{
    Stamina stamina;
    PlayerAnimationEvents animationEvents;
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
}