using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Roll : MonoBehaviour
{
    [SerializeField] float buffer = 0.2f;
    [SerializeField] int staminaCost = 20;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    Stamina stamina;
    InputInterface inputInterface;
    IEnumerator bufferCoroutine;
    Health health;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        stamina = GetComponent<Stamina>();
        health = GetComponent<Health>();
        inputInterface = GetComponent<InputInterface>();
    }
    void OnDodge()
    {
        if (inputInterface.PausedThisFrame) return;
        if (!animationEvents.ActionAvailable || stamina.IsExhausted)
        {
            if (bufferCoroutine != null)
                StopCoroutine(bufferCoroutine);
            bufferCoroutine = BufferDodge();
            StartCoroutine(bufferCoroutine);
        }
        else
            Dodge();
    }

    void Dodge()
    {
        if (!animationEvents.ActionAvailable || stamina.IsExhausted || health.IsDead)
            return;

        animator.SetTrigger("Roll");
        animationEvents.StopStaminaRegen();
        animationEvents.StartIFrame();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
        animationEvents.DisableActions();
        animationEvents.SyncRotation();
        animationEvents.BecomeIntangible();
        stamina.Remove(staminaCost);
    }

    IEnumerator BufferDodge()
    {
        for (float t = 0; t < buffer; t += Time.deltaTime)
        {
            yield return null;
            Dodge();
        }
    }
}
