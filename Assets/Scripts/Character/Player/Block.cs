using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

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
    [SerializeField]
    [Range(0f, 90f)]
    float blockAngle = 90f;
    bool isParryResetCoroutineRunning = false;
    PlayerController playerController;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    Stamina stamina;
    Stagger stagger;

    public bool IsBlocking { get; private set; } = false;
    public bool IsParrying { get; private set; } = false;
    public float DotBlockAngle { get => math.remap(0, 90, 1, 0, blockAngle); }
    private void Awake()
    {
        playerController = GetComponent<PlayerController>();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        stamina = GetComponent<Stamina>();
        stagger = GetComponent<Stagger>();
    }

    public void Update()
    {
        if ((animationEvents.ActionAvailable || stagger.IsStaggered && animator.GetBool("IsBlocking")) && playerController.BlockInput && !stamina.IsExhausted)
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
        yield return new WaitForSeconds(parryTime);
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

    public void StopBlocking()
    {
        IsBlocking = false;
        animator.SetBool("IsBlocking", false);
    }
}
