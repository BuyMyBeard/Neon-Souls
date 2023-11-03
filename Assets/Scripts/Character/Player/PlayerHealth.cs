using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerHealth : Health
{
    Stamina stamina;
    Stagger stagger;
    Block block;
    public bool isAutoParryOn = false;
    GameManager gameManager;
    PlayerAnimationEvents playerAnimationEvents;
    MeleeWeapon sword;
    private new void Awake()
    {
        base.Awake();
        displayHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();
        stamina = GetComponent<Stamina>();
        stagger = GetComponent<Stagger>();
        block = GetComponent<Block>();
        gameManager = FindObjectOfType<GameManager>();
        playerAnimationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        sword = GetComponentInChildren<MeleeWeapon>();
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
    [ContextMenu("Die")]
    protected override void Die()
    {
        base.Die();
        gameManager.PlayerDie();
        DetachSword();
    }
    private void DetachSword()
    {
        MeleeWeapon ragdollSword = Instantiate(sword, sword.transform.position, sword.transform.rotation, null);
        Rigidbody rb = ragdollSword.GetComponent<Rigidbody>();
        sword.gameObject.SetActive(false);
        ragdollSword.gameObject.layer = 16;
        Collider collider = ragdollSword.GetComponent<Collider>();
        ragdollSword.gameObject.AddComponent<DestroyOnRecharge>();
        ragdollSword.AddComponent<MeshCollider>().convex = true;
        Destroy(collider);
        Destroy(ragdollSword);
        rb.SetDensity(1);
        rb.isKinematic = false;
        rb.useGravity = true;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
    }
    public override void Recharge()
    {
        base.Recharge();
        displayHealthbar.Add(maxHealth, maxHealth);
        playerAnimationEvents.HidePotion();
        playerAnimationEvents.EnableActions();
        playerAnimationEvents.UnFreezeMovement();
        playerAnimationEvents.UnFreezeRotation();
        playerAnimationEvents.StopIFrame();
        animator.Play("Idle");
        GetComponent<CameraMovement>().SyncFollowTarget();
        sword.gameObject.SetActive(true);
    }
}
