using UnityEngine;

public abstract class AnimationEvents : MonoBehaviour
{
    protected Health health;
    protected Stagger stagger;
    protected MeleeAttack attack;
    protected FallApart fallApart;
    protected Sounds sounds;

    protected bool actionAvailable = true;
    public bool ActionAvailable { get => actionAvailable && !health.IsDead; set => actionAvailable = value; }
    protected virtual void Awake()
    {
        health = GetComponentInParent<Health>();
        attack = GetComponentInParent<MeleeAttack>();
        stagger = GetComponentInParent<Stagger>();
        fallApart = GetComponentInParent<FallApart>();
        sounds = GetComponentInParent<Sounds>();
    }
    public virtual void EnableActions() => ActionAvailable = true;
    public virtual void DisableActions() => ActionAvailable = false;
    public void StartIFrame() => health.invincible = true;
    public void StopIFrame() => health.invincible = false;
    public abstract void FreezeRotation();
    public abstract void UnFreezeRotation();
    public void InitWeaponCollider(AttackDef attackDef)
    {
        attack.InitWeaponCollider(attackDef);
    }
    public void DisableWeaponCollider(AttackDef attackDef) => attack.DisableWeaponCollider(attackDef);
    public virtual void DisableAllWeaponColliders() => attack.DisableAllWeaponColliders();
    public void EndStagger() => stagger.IsStaggered = false;
    public virtual void ResetAll()
    {
        EnableActions();
        StopIFrame();
        UnFreezeRotation();
        DisableAllWeaponColliders();
        EndStagger();
        RestoreTurnSpeed();
        Debug.Log("Resetted all");
    }
    public virtual void FallApart() => fallApart.Decompose();
    public virtual void FreezeMovement() { }
    public virtual void UnFreezeMovement() { }
    public abstract void ChangeTurnSpeed(float turnSpeed);
    public abstract void RestoreTurnSpeed();
    public virtual void PlaySound(SoundDef sound) => sounds.Play(sound.sound, sound.volume);
}
