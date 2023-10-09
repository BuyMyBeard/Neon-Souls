using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    Health health;
    ButtonPrompt buttonPrompt;
    Animator animator;
    private void Awake()
    {
        health = GetComponentInParent<Health>();
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
        animator = GetComponentInChildren<Animator>();

    }
    void OnInteract()
    {
        if (health.IsDead || buttonPrompt.currentPrompt == null)
            return;

        //Interact animation and handling
        buttonPrompt.Interact();
        animator.SetTrigger("Interact");
    } 
}
