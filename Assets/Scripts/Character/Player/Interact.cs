using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    ButtonPrompt buttonPrompt;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    private void Awake()
    {
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
    }
    void OnInteract()
    {
        if (!animationEvents.ActionAvailable || buttonPrompt.currentPrompt == null)
            return;

        buttonPrompt.HidePrompt();
        animator.SetTrigger(buttonPrompt.currentPrompt.animationTriggerName);
        animationEvents.DisableActions();
        animationEvents.StartIFrame();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
    }
    public void DoInteraction() => buttonPrompt.Interact();
}
