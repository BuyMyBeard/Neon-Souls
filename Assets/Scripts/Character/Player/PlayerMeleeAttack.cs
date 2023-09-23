using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

enum PlayerAttackType { Light, Heavy }

[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerInput))]
public class PlayerMeleeAttack : MeleeAttack
{
    [SerializeField] int lightAttackStaminaCost = 15;
    [SerializeField] int heavyAttackStaminaCost = 30;
    [SerializeField] int lightAttackDamage = 25;
    [SerializeField] int heavyAttackDamage = 50;
    Stamina stamina;
    PlayerAttackType attackType;
    protected override void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
    }
    void OnLightAttack()
    {
        if (!stamina.IsExhausted)
        {
            stamina.Remove(lightAttackStaminaCost);
            animator.SetTrigger("LightAttack");
            attackType = PlayerAttackType.Light;
        }
    }
    void OnHeavyAttack()
    {

    }
    protected override void DamageOpponent(Health opponentHealth)
    {
        if (attackType == PlayerAttackType.Light)
        {
            opponentHealth.InflictDamage(lightAttackDamage);
        }
    }

}
