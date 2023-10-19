using System.Collections;
using UnityEngine;

public class Stamina : MonoBehaviour, IRechargeable,IStat
{
    DisplayBar playerStaminabar;
    [SerializeField] int maxStamina = 100;
    [SerializeField] float currentStamina;
    [SerializeField] float staminaRegenRate = 100f;
    [SerializeField] float exhaustionTime = 0.5f;
    float exhaustionTimer = 0;
    IEnumerator exhaustionTimerCoroutine, regenStaminaCoroutine;
    bool exhaustionTimerStarted = false;
    bool isRegenerating = false;

    float ameliorateur;


    public bool IsExhausted { get => currentStamina <= 0; }
    public float Ameliorateur { get => ameliorateur; set { ameliorateur = value; } }

    public float Value => currentStamina;

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
    private void ResetStamina()
    {
        currentStamina = maxStamina;
    }
    /// <summary>
    /// Remove a defined amount of stamina
    /// </summary>
    /// <param name="value">Amount to remove</param>
    public void Remove(float value, bool syncLingeredValue = false)
    {
        if (!IsExhausted)
        {
            exhaustionTimer = 0;
            currentStamina -= value;
            if (currentStamina < 0)
                currentStamina = 0;
            playerStaminabar.Remove(value, maxStamina, true, syncLingeredValue);
            if (isRegenerating)
                StopCoroutine(regenStaminaCoroutine);
            if (!exhaustionTimerStarted)
            {
                exhaustionTimerCoroutine = ExhaustionTimer();
                StartCoroutine(exhaustionTimerCoroutine);
            }
        }
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

    public void Recharge()
    {
        ResetStamina();
        playerStaminabar.Add(maxStamina, maxStamina);
    }
}
