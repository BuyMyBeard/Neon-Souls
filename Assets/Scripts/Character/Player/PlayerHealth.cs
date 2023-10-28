using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    Stamina stamina;
    Stagger stagger;
    Block block;
    
    private new void Awake()
    {
        base.Awake();
        stamina = GetComponent<Stamina>();
        stagger = GetComponent<Stagger>();
        block = GetComponent<Block>();
    }
    public void InflictUnblockableDamage(int damage) => InflictDamage(damage);
    public void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition)
    {
        if (invincible) return;
        if (block.IsParrying && !stamina.IsExhausted && IsAttackerInFront(attackerPosition))
        {
            stamina.Remove(staminaBlockCost);
            block.ResetParryWindow();
            stagger.BlockHit(0.5f);
        }
        else if (block.IsBlocking && !stamina.IsExhausted && IsAttackerInFront(attackerPosition)) 
        {
            int damageReduced = (int) (damage * block.DamageReduction);
            stamina.Remove(staminaBlockCost);
            InflictDamage(damageReduced);
            stagger.BlockHit(1);
        }
        else
        {
            block.StopBlocking();
            InflictDamage(damage);
            stagger.BecomeStaggered(attackerPosition, 1);
        }
    }
    bool IsAttackerInFront(Transform attackerPosition)
    {
        if(attackerPosition != null)
        {
            Transform playerTransform = GetComponentInChildren<Animator>().transform;
            Vector3 directionToAttacker = attackerPosition.position - playerTransform.position;
            Vector2 flatPlayerForward = new Vector2(playerTransform.forward.x, playerTransform.forward.z).normalized;
            Vector2 flatDirectionToAttacker = new Vector2(directionToAttacker.x, directionToAttacker.z).normalized;
            Debug.Log(Vector2.Dot(flatPlayerForward, flatDirectionToAttacker));
            return Vector2.Dot(flatPlayerForward, flatDirectionToAttacker) > block.DotBlockAngle;
        }
        return false;
    }
}
