using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
//using UnityEditor.Experimental.GraphView;
using UnityEngine;
using UnityEngine.UIElements;

public enum BlockSound
{
    NoSound,
    SwordClash,
    PunchBlock
}
[Serializable]
struct BlockSoundToRandomSoundDef
{
    public BlockSound blockSound;
    public RandomSoundDef randomSoundDef;
}
public class PlayerHealth : Health, IStat
{
    [SerializeField] int upgradeHp = 10;
    [SerializeField] BlockSoundToRandomSoundDef[] blockSoundDefs;
    [SerializeField] BlockSoundToRandomSoundDef[] parrySoundDefs;
    public int Upgrade { get => upgradeHp;}
    public float Value => MaxHealth;
    Stamina stamina;
    Block block;
    public bool isAutoParryOn = false;
    GameManager gameManager;
    MeleeWeapon sword;
    ZoneTransitionManager zoneTransitionManager;
    MusicTransitionManager[] musicTransitionManagers;

    private new void Awake()
    {
        base.Awake();
        displayHealthbar = GameObject.FindGameObjectWithTag("PlayerHealthbar").GetComponent<DisplayBar>();
        stamina = GetComponent<Stamina>();
        block = GetComponent<Block>();
        gameManager = FindObjectOfType<GameManager>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        sword = GetComponentInChildren<MeleeWeapon>();
        zoneTransitionManager = FindObjectOfType<ZoneTransitionManager>();
        musicTransitionManagers = FindObjectsOfType<MusicTransitionManager>();
    }
    public override void HandleHealthbar(int damage)
    {
        displayHealthbar.Remove(damage, maxHealth, false);
    }
    private RandomSoundDef FindInStructArray(BlockSoundToRandomSoundDef[] array, BlockSound blockSound) => Array.Find(array, e => e.blockSound == blockSound).randomSoundDef;
    public override void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition, BlockSound blockSound = BlockSound.SwordClash)
    {
        
        if (invincible) return;
        {
        }

        if ((block.IsParrying || block.IsBlocking && isAutoParryOn) && !stamina.IsExhausted && IsAttackerInFront(attackerPosition))
        {
            if (blockSound != BlockSound.NoSound)
                animationEvents.PlaySoundRandom(FindInStructArray(parrySoundDefs, blockSound));
            Haptics.ImpactLight();
            stamina.Remove(staminaBlockCost);
            block.ResetParryWindow();
            stagger.BlockHit(0.5f);
            stamina.StopRegen();
        }
        else if (block.IsBlocking && !stamina.IsExhausted && IsAttackerInFront(attackerPosition)) 
        {
            if (blockSound != BlockSound.NoSound)
                animationEvents.PlaySoundRandom(FindInStructArray(blockSoundDefs, blockSound));
            Haptics.ImpactLight();
            int damageReduced = (int) (damage * block.DamageModifier);
            stamina.Remove(staminaBlockCost);
            InflictDamage(damageReduced);
            if (stamina.IsExhausted)
                block.GuardBreak();
            else
            {
                stagger.BlockHit(1);
                stamina.StopRegen();
            }
        }
        else
        {
            Haptics.Impact();
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
        foreach(var musicTransitionManager in musicTransitionManagers)
            musicTransitionManager.isInTrigger = false;
        zoneTransitionManager.FadeEverythingOut();

        gameManager.PlayerDie();
    }
    public override void Recharge(RechargeType rechargeType)
    {
        if (rechargeType == RechargeType.Respawn)
        {
            animator.Play("Idle");
            base.Recharge(rechargeType);
            displayHealthbar.Add(maxHealth, maxHealth);
            (animationEvents as PlayerAnimationEvents).HidePotion();
            animationEvents.EnableActions();
            animationEvents.UnFreezeMovement();
            animationEvents.UnFreezeRotation();
            animationEvents.StopIFrame();
            GetComponent<CameraMovement>().SyncFollowTarget();
            sword.gameObject.SetActive(true);
        }
        else
        {
            ResetHealth();
            displayHealthbar.Add(maxHealth, maxHealth);
        }
    }
    public void UpgradeStat(int upgrade)
    {
        maxHealth += upgrade * Upgrade;
    }
}
