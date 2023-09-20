using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    DisplayBar playerStaminabar;
    [SerializeField] int maxStamina = 100;
    [SerializeField] float currentStamina;
    [SerializeField] float staminaRegenRate = 100f;
    [SerializeField] int dodgeStamina = 20;
    [SerializeField] int lightAttackStamina = 25;
    [SerializeField] int heavyAttackStamina = 40;
    [SerializeField] float exhaustionTime = 0.5f;
    float exhaustionTimer = 0;
    IEnumerator exhaustionTimerCoroutine;
    IEnumerator regenStaminaCoroutine;
    bool exhaustionTimerStarted = false;
    bool isRegenerating = false;
    public bool IsExhausted { get => currentStamina <= 0; }

    private void Awake()
    {
        playerStaminabar = GameObject.FindGameObjectWithTag("DisplayedStamina").GetComponent<DisplayBar>();
    }
    private void OnEnable()
    {
        ResetStamina();
    }
    private void Update()
    {
        exhaustionTimer += Time.deltaTime;
    }
    private void OnDodge()//Maybe change to receive the stamina consumption from another component.
    {
        if(!IsExhausted)
        {
            exhaustionTimer = 0;
            currentStamina -= dodgeStamina;
            if (currentStamina < 0) 
                currentStamina = 0;
            playerStaminabar.Remove(dodgeStamina, maxStamina, true);
            if (isRegenerating)
                StopCoroutine(regenStaminaCoroutine);
            if (!exhaustionTimerStarted)
            {
                exhaustionTimerCoroutine = ExhaustionTimer();
                StartCoroutine(exhaustionTimerCoroutine);   
            }
        }

    }
    private void OnLightAttack()
    {

    }
    private void OnHeavyAttack()
    {

    }
    private void ResetStamina()
    {
        currentStamina = maxStamina;
    }
    private IEnumerator ExhaustionTimer()
    {
        exhaustionTimerStarted = true;
        yield return new WaitUntil(() => exhaustionTimer > exhaustionTime);
        exhaustionTimerStarted = false;
        regenStaminaCoroutine = RegenStamina();
        StartCoroutine(regenStaminaCoroutine);
    }
    private IEnumerator RegenStamina()
    {
        isRegenerating = true;
        while(currentStamina < maxStamina)
        {
            yield return null;
            float staminaToRegen = (staminaRegenRate * Time.deltaTime);
            currentStamina += staminaToRegen;
            playerStaminabar.Add(staminaToRegen, maxStamina);
        }
        ResetStamina();
        isRegenerating = false;
    }

}
