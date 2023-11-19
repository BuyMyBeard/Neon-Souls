using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Potions : MonoBehaviour, IRechargeable
{
    [SerializeField] public Material potionMat;
    [SerializeField] public GameObject potionObject;
    [SerializeField] public int maxPotions = 3;
    [SerializeField] public int currentPotions;
    [SerializeField] public int restoreValue = 60;
    [SerializeField] float timeToDrinkOne = .5f;
    Health health;
    IEnumerator refillHealthCoroutine;
    Animator animator;
    PlayerAnimationEvents animationEvents;
    Color currentColor;
    Material fluidMat;
    public float FillLevel
    {
        get => potionMat.GetFloat("_Height");
        set => potionMat.SetFloat("_Height", value);
    }
    void Awake()
    {
        fluidMat = potionObject.GetComponent<Renderer>().materials[1];
        currentColor = fluidMat.GetColor("_EmissionColor");
        var potionDisplay = GameObject.FindGameObjectWithTag("PotionDisplay").GetComponent<Image>();
        potionMat = new Material(potionMat);
        potionDisplay.material = potionMat;
        health = GetComponent<Health>();
        ResetPotions();
        animator = GetComponentInChildren<Animator>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        
    }
    private void Start()
    {
        HidePotion();
    }
    public void DrinkOnePotion()
    {
        currentPotions--;
        if (currentPotions <= 0)
        {
            fluidMat.SetColor("_EmissionColor", Color.black);
            Color c = fluidMat.GetColor("_BaseColor");
            c.a = 0;
            fluidMat.SetColor("_BaseColor", c);
        }
        StartCoroutine(UpdateFillLevelProgressively());
        refillHealthCoroutine = RefillHealth();
        StartCoroutine(refillHealthCoroutine);

    }
    public void ResetPotions()
    {
        currentPotions = maxPotions;
        UpdateFillLevel();
    }
    public void UpdateFillLevel() => FillLevel = (float) currentPotions / maxPotions;
    IEnumerator UpdateFillLevelProgressively()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / timeToDrinkOne)
        {
            yield return null;
            FillLevel = Mathf.Lerp(currentPotions + 1, currentPotions, t) / maxPotions;
        }
        UpdateFillLevel();
    }
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
        fluidMat.SetColor("_EmissionColor", currentColor);
        Color c = fluidMat.GetColor("_BaseColor");
        c.a = 1;
        fluidMat.SetColor("_BaseColor", c);
        ResetPotions();
    }
    public void ShowPotion()
    {
        potionObject.SetActive(true);
    }
    public void HidePotion()
    {
        potionObject.SetActive(false);
    }

    public void SetLiquidColor(Color color)
    {
        currentColor = color;
        if (currentPotions > 0)
            fluidMat.SetColor("_EmissionColor", color);
    }
}
