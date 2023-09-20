using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{

    [SerializeField] Material potionMat;
    [SerializeField] int maxPotions = 3;
    [SerializeField] int currentPotions;
    [SerializeField] int restoreValue = 60;
    [SerializeField] float timeToDrinkOne = .5f;
    Health health;
    bool isRefillingHealth = false;
    IEnumerator refillHealthCoroutine;
    public float FillLevel
    {
        get => potionMat.GetFloat("_Height");
        set => potionMat.SetFloat("_Height", value);
    }
    void Awake()
    {
        health = GetComponent<Health>();
        ResetPotions();
    }
    public void DrinkOnePotion()
    {
        if (currentPotions > 0 && !isRefillingHealth)
        {
            currentPotions--;
            UpdateFillLevel();
            refillHealthCoroutine = RefillHealth();
            StartCoroutine(refillHealthCoroutine);
        }
    }
    public void ResetPotions()
    {
        currentPotions = maxPotions;
        UpdateFillLevel();
    }
    void UpdateFillLevel() => FillLevel = (float) currentPotions / maxPotions;
    void OnConsumable()
    {
        DrinkOnePotion();
    }
    void OnInteract()
    {
        ResetPotions();
    }
    IEnumerator RefillHealth()
    {
        isRefillingHealth = true;
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
        isRefillingHealth = false;
    }
}
