using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    Health health;
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    ButtonPrompt buttonPrompt;

    CharacterController playerCharacter;
    private void Awake()
    {
        health = GetComponentInParent<Health>();
        playerMovement = GetComponentInParent<PlayerMovement>(); 
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
        cameraMovement = FindObjectOfType<CameraMovement>();
        playerCharacter = FindObjectOfType<CharacterController>();
    }
    void OnInteract()
    {
        if (health.IsDead || buttonPrompt.currentPrompt == null)
            return;
        //IFrame
        health.invincible = true;
        playerMovement.frozen = true;
        playerCharacter.enabled = false;
        Debug.Log("IFrameStart");
        //Interact animation and handling
        buttonPrompt.Interact();
        Animator animator = GetComponentInChildren<Animator>();
        animator.SetTrigger("Interact");
    } 

    public void EndInteract()
    {
        health.invincible = false;
        playerMovement.frozen = false;
        playerCharacter.enabled = true;
        Debug.Log("IFrameStop");
    }
}
