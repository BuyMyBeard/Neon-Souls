using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class PlayerHealth : Health, IStat
{
    [SerializeField] int upgradeHp = 10;
    public int Upgrade { get => upgradeHp;}
    public float Value => MaxHealth;
    Stamina stamina;
    Block block;
    public bool isAutoParryOn = false;
    GameManager gameManager;
    MeleeWeapon sword;
    [SerializeField] RandomSoundDef blockDef;
    [SerializeField] RandomSoundDef parryDef;

    private new void Awake()
    {
        base.Awake();
        displayHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();
        stamina = GetComponent<Stamina>();
        block = GetComponent<Block>();
        gameManager = FindObjectOfType<GameManager>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        sword = GetComponentInChildren<MeleeWeapon>();
    }
    public override void HandleHealthbar(int damage)
    {
        displayHealthbar.Remove(damage, maxHealth, true);//TODO: change to false.
    }
    public override void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition)
    {
        if (invincible) return;
        if (block.IsParrying)
            animationEvents.PlaySoundRandom(parryDef);
        else if (block.IsBlocking)
            animationEvents.PlaySoundRandom(blockDef);

        if ((block.IsParrying || block.IsBlocking && isAutoParryOn) && !stamina.IsExhausted && IsAttackerInFront(attackerPosition))
        {
            // Haptics.ImpactLight();
            stamina.Remove(staminaBlockCost);
            block.ResetParryWindow();
            stagger.BlockHit(0.5f);
        }
        else if (block.IsBlocking && !stamina.IsExhausted && IsAttackerInFront(attackerPosition)) 
        {
            // Haptics.Impact();
            int damageReduced = (int) (damage * block.DamageModifier);
            stamina.Remove(staminaBlockCost);
            InflictDamage(damageReduced);
            stagger.BlockHit(1);
        }
        else
        {
            // Haptics.ImpactHeavy();
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
    }
    public override void Recharge()
    {
        if (IsDead)
            animator.Play("Idle");
        base.Recharge();
        displayHealthbar.Add(maxHealth, maxHealth);
        (animationEvents as PlayerAnimationEvents).HidePotion();
        animationEvents.EnableActions();
        animationEvents.UnFreezeMovement();
        animationEvents.UnFreezeRotation();
        animationEvents.StopIFrame();
        GetComponent<CameraMovement>().SyncFollowTarget();
        sword.gameObject.SetActive(true);
    }
    public void UpgradeStat(int upgrade)
    {
        maxHealth += upgrade * Upgrade;
    }
}
