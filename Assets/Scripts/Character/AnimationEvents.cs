using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : MonoBehaviour
{
    Interact interact;
    Health health;
    CharacterController characterController;
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    MeleeAttack attack;
    Potions potions;
    Animator animator;
    private void Awake()
    {
        interact = GetComponentInParent<Interact>();
        health = GetComponentInParent<Health>();
        characterController = GetComponentInParent<CharacterController>();
        playerMovement = GetComponentInParent<PlayerMovement>();
        cameraMovement = GetComponentInParent<CameraMovement>();
        attack = GetComponentInParent<MeleeAttack>();
        potions = GetComponentInParent<Potions>();
        animator = GetComponentInParent<Animator>();
    }

    public void StartIFrame() => health.invincible = true;
    public void StopIFrame() => health.invincible = false;
    // public void FreezeMovement() { characterController.enabled = false; playerMovement.movementFrozen = true; }
    // public void UnFreezeMovement() { characterController.enabled = true; playerMovement.movementFrozen = false; }
    public void FreezeCamera() => cameraMovement.enabled = false;
    public void UnFreezeCamera() => cameraMovement.enabled = true;
    public void FreezeRotation() => animator.SetBool("RotationFrozen", true);
    public void UnFreezeRotation() => animator.SetBool("RotationFrozen", false);
    public void EnableWeaponCollider() => attack.EnableWeaponCollider();
    public void DisableWeaponCollider() => attack.DisableWeaponCollider();
    public void DrinkPotion() => potions.DrinkOnePotion();
}
