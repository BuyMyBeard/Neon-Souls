public class PlayerAnimationEvents : AnimationEvents
{
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    Potions potions;
    Spells spells;
    Stamina stamina;
    PlayerDeath playerDeath;
    Interact interact;

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponentInParent<PlayerMovement>();
        cameraMovement = GetComponentInParent<CameraMovement>();
        potions = GetComponentInParent<Potions>();
        spells = GetComponentInParent<Spells>();
        stamina = GetComponentInParent<Stamina>();
        playerDeath = GetComponentInParent<PlayerDeath>();
        interact = GetComponentInParent<Interact>();
    }
    
    public override void EnableActions()
    {
        base.EnableActions();
        stamina.StartRegen();
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
    public void FocusSpell() => spells.FocusFireball();
    public void ThrowSpell() => spells.ThrowFireball();
    public void StopStaminaRegen() => stamina.StopRegen();
    public override void ResetAll()
    {
        base.ResetAll();
        UnFreezeMovement();
        UnFreezeCamera();
        HidePotion();
        RestoreMovement();
    }
    public override void FallApart() => playerDeath.Decompose();
    public void DoInteraction() => interact.DoInteraction();

}
