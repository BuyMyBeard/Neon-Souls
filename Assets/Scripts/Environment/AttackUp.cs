using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : TimedPowerup
{
    [SerializeField] int meleeBonus;
    [SerializeField] int spellsBonus;

    MeleeAttack meleeAttack;
    Spells spells;

    protected override void Awake()
    {
        base.Awake();
        meleeAttack = player.GetComponentInParent<MeleeAttack>();
        spells = player.GetComponentInParent<Spells>();
    }
    public override void Apply()
    {
        base.Apply();
        meleeAttack.bonusDamage += meleeBonus;
        spells.damageScalingBonus += spellsBonus;
    }

    public override void Revert()
    {
        base.Revert();
        meleeAttack.bonusDamage -= meleeBonus;
        spells.damageScalingBonus -= spellsBonus;
    }
}
