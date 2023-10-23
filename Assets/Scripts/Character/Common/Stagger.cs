using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stagger : MonoBehaviour
{
    Animator animator;
    PlayerAnimationEvents animationEvents;
    public bool IsStaggered { get; private set; } = false;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    public void Update()
    {
        //if (!IsStaggered)
        //{
        //    animator.SetBool("IsStagggered", true);
        //    animationEvents.ReduceMovement();
        //    IsStaggered = true;
        //}
        //else
        //{
        //    animator.SetBool("IsStagggered", false);
        //    animationEvents.RestoreMovement();
        //    IsStaggered = false;
        //}
    }


}
