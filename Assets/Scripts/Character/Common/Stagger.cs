using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class Stagger : MonoBehaviour
{
    Animator animator;
    AnimationEvents animationEvents;
    Health health;
    bool isStaggered = false;
    public UnityEvent onStagger = new();
    public bool IsStaggered
    {
        get => isStaggered;
        set
        {
            isStaggered = value;
            animator.SetBool("IsStaggered", value);
        }
    }

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<AnimationEvents>();
        health = GetComponent<Health>();
    }
    public void BecomeStaggered(Transform target, float knockback = 1)
    {
        if (health.IsDead)
            return;

        animationEvents.ResetAll();
        animationEvents.FreezeRotation();
        animationEvents.FreezeMovement();
        animationEvents.DisableActions();

        Vector3 playerPlanePos = new(animator.transform.position.x,0,animator.transform.position.z);
        Vector3 targetPlanePos = new(target.transform.position.x, 0, target.transform.position.z);
        Vector3 targetDir = animator.transform.InverseTransformDirection((targetPlanePos - playerPlanePos).normalized);
        IsStaggered = true;
        onStagger.Invoke();

        animator.SetTrigger("Stagger");
        animator.SetFloat("StaggerX", targetDir.x);
        animator.SetFloat("StaggerY", targetDir.z);
        animator.SetFloat("Knockback", knockback);
    }
    public void BlockHit(float knockback = 1)
    {
        if (health.IsDead)
            return;
        animationEvents.ResetAll();
        if (animationEvents is PlayerAnimationEvents)
        {
            (animationEvents as PlayerAnimationEvents).StopStaminaRegen();
        }
        animationEvents.FreezeRotation();
        animationEvents.FreezeMovement();
        animationEvents.DisableActions();
        animationEvents.DisableAllWeaponColliders();
        IsStaggered = true;
        animator.SetTrigger("BlockHit");
        animator.SetFloat("Knockback", knockback);
    }
}
