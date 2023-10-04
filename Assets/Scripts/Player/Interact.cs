using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interact : MonoBehaviour
{
    GameManager gameManager;
    Health health;
    ButtonPrompt buttonPrompt;
    private void Awake()
    {
        health = GetComponentInParent<Health>();
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
        gameManager = FindObjectOfType<GameManager>();
    }
    void OnInteract()
    {
        if (health.IsDead || buttonPrompt.currentPrompt == null)
            return;

        gameManager.StartIFrame();
        gameManager.FreezeCamera();
        gameManager.FreezePlayer();

        //Interact animation and handling
        buttonPrompt.Interact();
        GetComponentInChildren<Animator>().SetTrigger("Interact");
    } 

    public void EndInteract()
    {
        gameManager.StopIFrame();
        gameManager.UnFreezePlayer();
        gameManager.UnFreezeCamera();
    }
}
