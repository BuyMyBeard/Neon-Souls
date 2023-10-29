using JetBrains.Annotations;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationEvents : AnimationEvents
{
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    Potions potions;

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponentInParent<PlayerMovement>();
        cameraMovement = GetComponentInParent<CameraMovement>();
        potions = GetComponentInParent<Potions>();
    }

    public void FreezeMovement() => playerMovement.movementFrozen = true;
    public void UnFreezeMovement() => playerMovement.movementFrozen = false;
    public void FreezeCamera() => cameraMovement.enabled = false;
    public void UnFreezeCamera() => cameraMovement.enabled = true;
    public override void FreezeRotation() => playerMovement.rotationFrozen = true;
    public override void UnFreezeRotation() => playerMovement.rotationFrozen = false;
    public void DrinkPotion() => potions.DrinkOnePotion();
    public void ReduceMovement() => playerMovement.movementReduced = true;
    public void RestoreMovement() => playerMovement.movementReduced = false;
    public void SyncRotation() => playerMovement.SyncRotation();
    public void ShowPotion() => potions.ShowPotion();
    public void HidePotion() => potions.HidePotion();
    public override void ResetAll()
    {
        base.ResetAll();
        UnFreezeMovement();
        UnFreezeCamera();
        HidePotion();
        RestoreMovement();
    }
}
