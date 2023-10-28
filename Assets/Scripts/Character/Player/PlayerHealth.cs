using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health, IRechargeable
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
            return Vector3.Dot(playerTransform.forward.normalized, directionToAttacker.normalized) > 0;
        }
        return false;
    }
    public virtual void Recharge()
    {
        ResetHealth();
        displayHealthbar.Add(maxHealth, maxHealth);
        if (CompareTag("Player"))
        {
            animationEvents.HidePotion();
            animationEvents.EnableActions();
            animationEvents.UnFreezeMovement();
            animationEvents.UnFreezeRotation();
            animationEvents.StopIFrame();
            animator.Play("Idle");
            GetComponent<CameraMovement>().SyncFollowTarget();
        }
    }
}
