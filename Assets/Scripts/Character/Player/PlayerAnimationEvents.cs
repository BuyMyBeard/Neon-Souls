using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Health health;
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    MeleeAttack attack;
    Potions potions;

    bool actionAvailable = true;
    public bool ActionAvailable { get => actionAvailable; }
    private void Awake()
    {
        health = GetComponentInParent<Health>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        cameraMovement = GetComponentInParent<CameraMovement>();
        attack = GetComponentInParent<MeleeAttack>();
        potions = GetComponentInParent<Potions>();
    }
    public void EnableActions() => actionAvailable = true;
    public void DisableActions() => actionAvailable = false;
    public void StartIFrame() => health.invincible = true;
    public void StopIFrame() => health.invincible = false;
    public void FreezeMovement() => playerMovement.movementFrozen = true;
    public void UnFreezeMovement() => playerMovement.movementFrozen = false;
    public void FreezeCamera() => cameraMovement.enabled = false;
    public void UnFreezeCamera() => cameraMovement.enabled = true;
    public void FreezeRotation() => playerMovement.rotationFrozen = true;
    public void UnFreezeRotation() => playerMovement.rotationFrozen = false;
    public void EnableWeaponCollider() => attack.EnableWeaponCollider();
    public void DisableWeaponCollider() => attack.DisableWeaponCollider();
    public void DrinkPotion() => potions.DrinkOnePotion();
    public void ReduceMovement() => playerMovement.movementReduced = true;
    public void RestoreMovement() => playerMovement.movementReduced = false;
    public void SyncRotation() => playerMovement.SyncRotation();
    public void ShowPotion() => potions.ShowPotion();
    public void HidePotion() => potions.HidePotion();
}
