using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Stamina))]
[RequireComponent(typeof(PlayerController))]
public class Block : MonoBehaviour
{
    [SerializeField]
    float parryTime = 0.5f;
    [SerializeField]
    float parryResetTime = 1f;
    [SerializeField]
    public float DamageReduction = 0.20f;
    bool isParryResetCoroutineRunning = false;
    PlayerController playerController;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    Stamina stamina;

    public bool IsBlocking { get; private set; } = false;
    public bool IsParrying { get; private set; } = false;

    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        stamina = GetComponent<Stamina>();
    }

    public void Update()
    {
        if (animationEvents.ActionAvailable && playerController.BlockInput && !stamina.IsExhausted)
        {
            animator.SetBool("IsBlocking", true);
            animationEvents.ReduceMovement();
            IsBlocking = true;
        }
        else
        {
            animator.SetBool("IsBlocking", false);
            animationEvents.RestoreMovement();
            IsBlocking = false;
        }
    }
    public void OnParry()
    {
        if (!isParryResetCoroutineRunning)
        {
            StartCoroutine(ParryTimeCoroutine());
            StartCoroutine(ParryResetCoroutine());
        }
    }
    IEnumerator ParryTimeCoroutine()
    {
        IsParrying = true;
        Debug.Log("parry start");
        yield return new WaitForSeconds(parryTime);
        Debug.Log("parry ended");
        IsParrying = false;
    }
    IEnumerator ParryResetCoroutine()
    {
        isParryResetCoroutineRunning = true;
        yield return new WaitForSeconds(parryResetTime);
        isParryResetCoroutineRunning = false;
    }
    public void ResetParryWindow()
    {
        StopAllCoroutines();
        isParryResetCoroutineRunning = false;
        IsParrying = false;
    }
}
