using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : Health
{
    Stamina stamina;
    Stagger stagger;
    Block block;
    public bool isAutoParryOn = false;
    private new void Awake()
    {
        base.Awake();
        displayHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();
        stamina = GetComponent<Stamina>();
        stagger = GetComponent<Stagger>();
        block = GetComponent<Block>();
    }
    public override void HandleHealthbar(int damage)
    {
        displayHealthbar.Remove(damage, maxHealth, true);//TODO: change to false.
    }
    public override void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition)
    {
        if (invincible) return;
        if ((block.IsParrying || block.IsBlocking && isAutoParryOn) && !stamina.IsExhausted && IsAttackerInFront(attackerPosition))
        {
            stamina.Remove(staminaBlockCost);
            block.ResetParryWindow();
            stagger.BlockHit(0.5f);
        }
        else if (block.IsBlocking && !stamina.IsExhausted && IsAttackerInFront(attackerPosition)) 
        {
            int damageModifier = (int) (damage * block.DamageModifier);
            stamina.Remove(staminaBlockCost);
            InflictDamage(damageModifier);
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
            return Vector2.Dot(flatPlayerForward, flatDirectionToAttacker) > block.DotBlockAngle;
        }
        return false;
    }
    public override void Recharge()
    {
        base.Recharge();
        displayHealthbar.Add(maxHealth, maxHealth);
        animationEvents.HidePotion();
        animationEvents.EnableActions();
        animationEvents.UnFreezeMovement();
        animationEvents.UnFreezeRotation();
        animationEvents.StopIFrame();
        animator.Play("Idle");
        GetComponent<CameraMovement>().SyncFollowTarget();
    }
}
