using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health, IRechargeable
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
            Haptics.ImpactLight();
            stamina.Remove(staminaBlockCost);
            block.ResetParryWindow();
            stagger.BlockHit(0.5f);
        }
        else if (block.IsBlocking && !stamina.IsExhausted && IsAttackerInFront(attackerPosition)) 
        {
            Haptics.Impact();
            int damageReduced = (int) (damage * block.DamageReduction);
            stamina.Remove(staminaBlockCost);
            InflictDamage(damageReduced);
            stagger.BlockHit(1);
        }
        else
        {
            Haptics.ImpactHeavy();
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
            return Vector2.Dot(flatPlayerForward, flatDirectionToAttacker) > block.DotBlockAngle;
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
            animator.SetTrigger("Reset");
            GetComponent<CameraMovement>().SyncFollowTarget();
        }
    }
}
