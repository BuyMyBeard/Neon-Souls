using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerAttackType { Light, Heavy }

[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMeleeAttack : MeleeAttack,IStat
{
    [SerializeField] int lightAttackStaminaCost = 15;
    [SerializeField] int heavyAttackStaminaCost = 30;
    [SerializeField] int lightAttackDamage = 25;
    [SerializeField] int heavyAttackDamage = 50;
    [SerializeField] int ameliorateur = 0;

    Stamina stamina;
    PlayerAnimationEvents animationEvents;
    PlayerAttackType attackType;

    public float Value => lightAttackDamage;
    public int Ameliorateur => ameliorateur;
    protected override void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    void OnLightAttack()
    {
        if (!animationEvents.ActionAvailable || stamina.IsExhausted) return;
        
        stamina.Remove(lightAttackStaminaCost);
        animator.SetTrigger("LightAttack");
        attackType = PlayerAttackType.Light;
        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
    }
    void OnHeavyAttack()
    {
        if (!animationEvents.ActionAvailable || stamina.IsExhausted) return;

        stamina.Remove(heavyAttackStaminaCost);
        animator.SetTrigger("HeavyAttack");
        attackType = PlayerAttackType.Heavy;
        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
    }
    protected override void DamageOpponent(Health opponentHealth)
    {
        if (attackType == PlayerAttackType.Light)
        {
            opponentHealth.InflictDamage(lightAttackDamage);
        }
        else if (attackType == PlayerAttackType.Heavy)
        {
            opponentHealth.InflictDamage(heavyAttackDamage);
        }
    }

    public void UpgradeStat(int nbAmelioration)
    {
        lightAttackDamage += ameliorateur * nbAmelioration;
        heavyAttackDamage += ameliorateur * nbAmelioration;
    }
}