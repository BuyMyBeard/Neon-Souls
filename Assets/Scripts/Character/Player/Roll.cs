using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    [SerializeField] int staminaCost = 20;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    Stamina stamina;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        stamina = GetComponent<Stamina>();
    }
    void OnDodge()
    {
        if (!animationEvents.ActionAvailable || stamina.IsExhausted)
            return;

        animator.SetTrigger("Roll");
        animationEvents.StartIFrame();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
        animationEvents.DisableActions();
        animationEvents.SyncRotation();
        stamina.Remove(staminaCost);
    }
}
