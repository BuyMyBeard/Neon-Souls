using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    Stamina stamina;
    Block block;
    
    private new void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
        block = GetComponent<Block>();
    }
    public void InflictUnblockableDamage(int damage) => InflictDamage(damage);
    public void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition)
    {  
        if (block.IsParrying && !stamina.IsExhausted && IsAttackerInFront(attackerPosition))
        {
            stamina.Remove(staminaBlockCost);
            block.ResetParryWindow();
        }
        else if (block.IsBlocking && !stamina.IsExhausted && IsAttackerInFront(attackerPosition)) 
        {
            int damageReduced = (int) (damage * block.DamageReduction);
            stamina.Remove(staminaBlockCost);
            base.InflictDamage(damageReduced);
        }
        else
        {
            InflictDamage(damage);
        }
    }
    bool IsAttackerInFront(Transform attackerPosition)
    {
        if(attackerPosition != null)
        {
            Transform playerTransform = GetComponentInChildren<Animator>().transform;
            Vector3 directionToAttacker = attackerPosition.position - playerTransform.position;
            if (Vector3.Dot(playerTransform.forward.normalized, directionToAttacker.normalized) > 0)
                return true;
            return false;
        }
        return false;
    }
}
