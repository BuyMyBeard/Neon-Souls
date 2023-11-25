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
    InputInterface inputInterface;
    [SerializeField] int dmgUpgrade = 0;
    public float Value => baseDamage;
    public int Upgrade => dmgUpgrade;
    bool canComboLight = false;
    bool canComboHeavy = false;
    protected override void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        inputInterface = GetComponent<InputInterface>();
    }
    public override void InitWeaponCollider(AttackDef attackDef)
    {
        base.InitWeaponCollider(attackDef);
        stamina.Remove(attackDef.staminaCost);
    }
    private void Update()
    {
        if (stamina.IsExhausted)
        {
            animator.ResetTrigger("LightAttack");
            animator.ResetTrigger("HeavyAttack");
        }
    }
    void OnLightAttack()
    {
        if ((!canComboLight && !animationEvents.ActionAvailable) || stamina.IsExhausted || health.IsDead || inputInterface.PausedThisFrame) return;

        animator.ResetTrigger("LightAttack");
        animator.SetTrigger("LightAttack");
        canComboLight = true;
    }
    void OnHeavyAttack()
    {
        if (!canComboHeavy && !animationEvents.ActionAvailable || stamina.IsExhausted || health.IsDead || inputInterface.PausedThisFrame) return;

        animator.ResetTrigger("HeavyAttack");
        animator.SetTrigger("HeavyAttack");
        canComboHeavy = true;
    }
    public void UpgradeStat(int nbAmelioration)
    {
        bonusDamage += nbAmelioration * dmgUpgrade;
    }
    public void ResetCombo()
    {
        canComboLight = false; 
        canComboHeavy = false;
        animator.ResetTrigger("LightAttack");
        animator.ResetTrigger("HeavyAttack");
        animator.SetBool("Attacking", false);
    }
}