using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagger : MonoBehaviour
{
    Animator animator;
    PlayerAnimationEvents animationEvents;
    Health health;
    public bool IsStaggered { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        health = GetComponent<Health>();
    }
    public void BecomeStaggered(Transform target)
    {
        if (health.IsDead)
            return;
        animationEvents.FreezeRotation();
        animationEvents.FreezeMovement();
        animationEvents.DisableActions();
        Vector3 playerPlanePos = new Vector3(animator.transform.position.x,0,animator.transform.position.z);
        Vector3 targetPlanePos = new Vector3(target.transform.position.x, 0, target.transform.position.z);
        Vector3 targetDir = animator.transform.InverseTransformDirection((targetPlanePos - playerPlanePos).normalized);

        animator.SetTrigger("Staggered");
        animator.SetFloat("StaggerX", targetDir.x);
        animator.SetFloat("StaggerY", targetDir.z);
    }
    public void BlockHit()
    {
        if (health.IsDead)
            return;
        animationEvents.FreezeRotation();
        animationEvents.FreezeMovement();
        animationEvents.DisableActions();
        animator.SetTrigger("BlockHit");
    }
}
