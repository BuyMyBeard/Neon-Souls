using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(LockOn))]
[RequireComponent(typeof(Mana))]
[RequireComponent(typeof(Stamina))]
public class Spells : MonoBehaviour,IStat
{
    [SerializeField] GameObject fireballPrefab;
    [SerializeField] Transform attachPoint;
    [Range(1f, 20f)]
    [SerializeField] float throwSpeed = 1.0f;
    [Range(0f, 1f)]
    [SerializeField] float lobeFactor = 0f;
    [SerializeField] int staminaCost;
    [SerializeField] int manaCost;
    [SerializeField] int upgradeDmg;
    Animator animator;
    Fireball fireball;
    PlayerAnimationEvents animationEvents;
    LockOn lockOn;
    Stamina stamina;
    Mana mana;
    public int damageScalingBonus = 0;

    public float Value => fireballPrefab.GetComponent<Fireball>().BaseDamage;

    public int Upgrade => upgradeDmg;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        lockOn = GetComponent<LockOn>();
        stamina = GetComponent<Stamina>();
        mana = GetComponent<Mana>();
    }
    void OnCastSpell()
    {
        if (!animationEvents.ActionAvailable || !mana.CanCast(manaCost)) return;

        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
        animationEvents.StopStaminaRegen();
        animator.SetTrigger("ThrowSpell");
    }

    public void FocusFireball()
    {
        fireball = Instantiate(fireballPrefab, attachPoint).GetComponent<Fireball>();
        fireball.damageScalingBonus = damageScalingBonus;
        stamina.Remove(staminaCost);
        mana.Remove(manaCost);
    }

    public void ThrowFireball()
    {
        if(fireball != null)
        {
            fireball.transform.parent = null;
            Rigidbody rb = fireball.GetComponent<Rigidbody>();
            Vector3 throwDirection;
            if (lockOn.IsLocked)
            {
                throwDirection = (lockOn.TargetEnemy.transform.position - fireball.transform.position).normalized;
            }
            else
            {
                throwDirection = animator.transform.forward;
            }
            rb.AddForce(throwDirection * throwSpeed + throwSpeed * lobeFactor * Vector3.up, ForceMode.VelocityChange);
            fireball.GetComponent<Collider>().enabled = true;
            fireball.thrown = true;
        }
    }

    public void UpgradeStat(int nbAmelioration)
    {
        damageScalingBonus += upgradeDmg * nbAmelioration;
    }
    public void ClearHand()
    {
        foreach (Transform t in attachPoint)
        {
            Destroy(t.gameObject);
        }
    }
}
