public class PlayerAnimationEvents : AnimationEvents
{
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    Potions potions;
    Spells spells;
    Stamina stamina;

    protected override void Awake()
    {
        base.Awake();
        playerMovement = GetComponentInParent<PlayerMovement>();
        cameraMovement = GetComponentInParent<CameraMovement>();
        potions = GetComponentInParent<Potions>();
        spells = GetComponentInParent<Spells>();
        stamina = GetComponentInParent<Stamina>();
    }
    
    public override void EnableActions()
    {
        base.EnableActions();
        stamina.StartRegen();
    }
    public override void FreezeMovement() => playerMovement.movementFrozen = true;
    public override void UnFreezeMovement() => playerMovement.movementFrozen = false;
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
}
