using UnityEngine;

public abstract class AnimationEvents : MonoBehaviour
{
    protected Health health;
    protected Stagger stagger;
    protected MeleeAttack attack;

    public bool ActionAvailable { get; protected set; } = true;
    protected virtual void Awake()
    {
        health = GetComponentInParent<Health>();
        attack = GetComponentInParent<MeleeAttack>();
        stagger = GetComponentInParent<Stagger>();
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
    public void DisableAllWeaponColliders() => attack.DisableAllWeaponColliders();
    public void EndStagger() => stagger.IsStaggered = false;
    public virtual void ResetAll()
    {
        EnableActions();
        StopIFrame();
        UnFreezeRotation();
        DisableAllWeaponColliders();
        EndStagger();
    }
}
