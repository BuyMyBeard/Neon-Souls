using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    ButtonPrompt buttonPrompt;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    InputInterface inputInterface;
    private void Awake()
    {
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        inputInterface = GetComponent<InputInterface>();
    }
    void OnInteract()
    {
        if (!animationEvents.ActionAvailable || buttonPrompt.currentPrompt == null || inputInterface.PausedThisFrame)
            return;

        buttonPrompt.HidePrompt();
        animator.SetTrigger(buttonPrompt.currentPrompt.animationTriggerName);
        animationEvents.DisableActions();
        animationEvents.StartIFrame();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
        animationEvents.BecomeIntangible();
    }
    public void DoInteraction() => buttonPrompt.Interact();
}
