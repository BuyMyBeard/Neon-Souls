using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour ,IRechargeable
{

    [SerializeField] Material potionMat;
    [SerializeField] int maxPotions = 3;
    [SerializeField] int currentPotions;
    [SerializeField] int restoreValue = 60;
    [SerializeField] float timeToDrinkOne = .5f;
    Health health;
    IEnumerator refillHealthCoroutine;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    public float FillLevel
    {
        get => potionMat.GetFloat("_Height");
        set => potionMat.SetFloat("_Height", value);
    }
    void Awake()
    {
        health = GetComponent<Health>();
        ResetPotions();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        
    }
    public void DrinkOnePotion()
    {
        currentPotions--;
        UpdateFillLevel();
        refillHealthCoroutine = RefillHealth();
        StartCoroutine(refillHealthCoroutine);

    }
    public void ResetPotions()
    {
        currentPotions = maxPotions;
        UpdateFillLevel();
    }
    void UpdateFillLevel() => FillLevel = (float) currentPotions / maxPotions;
    void OnConsumable()
    {
        if (!animationEvents.ActionAvailable) return;

        if (currentPotions > 0)
            animator.SetTrigger("Drink");
        else
            animator.SetTrigger("DrinkEmpty");

        animationEvents.DisableActions();
        animationEvents.ReduceMovement();
    }
    IEnumerator RefillHealth()
    {
        float t = 0;
        while (t <= timeToDrinkOne)
        {
            t += Time.deltaTime;
            float healthRegained;
            if (t > timeToDrinkOne)
            {
                float prev = t - Time.deltaTime;
                healthRegained = (timeToDrinkOne - prev) * restoreValue / timeToDrinkOne;
            }
            else 
                healthRegained = Time.deltaTime * restoreValue / timeToDrinkOne;
            health.Heal(healthRegained);
            yield return null;
        }
        health.Round();
    }

    public void Recharge()
    {
        ResetPotions();
    }
}
