using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackUp : TimedPowerup
{
    [SerializeField] float meleeBonus;
    [SerializeField] float spellsBonus;

    PlayerMeleeAttack meleeAttack;
    Spells spells;
    int meleeBonusGiven;
    int spellsBonusGiven;

    protected override void Awake()
    {
        base.Awake();
        meleeAttack = player.GetComponentInParent<PlayerMeleeAttack>();
        spells = player.GetComponentInParent<Spells>();
    }
    public override void Apply()
    {
        base.Apply();
        meleeBonusGiven = (int)(meleeAttack.Value * meleeBonus);
        spellsBonusGiven = (int)(meleeAttack.Value * spellsBonus);
        meleeAttack.bonusDamage += meleeBonusGiven;
        spells.damageScalingBonus += spellsBonusGiven;
    }

    public override void Revert()
    {
        base.Revert();
        meleeAttack.bonusDamage -= meleeBonusGiven;
        spells.damageScalingBonus -= spellsBonusGiven;
    }
}
