using System.Collections;
using UnityEngine;

public class Stamina : MonoBehaviour, IRechargeable,IStat
{
    DisplayBar playerStaminabar;
    [SerializeField] int maxStamina = 100;
    [SerializeField] float currentStamina;
    [SerializeField] float staminaRegenRate = 100f;
    [SerializeField] float blockingRegenMultiplier = .5f;
    [SerializeField] int staminaRequiredToRun = 20;
    [SerializeField]int upgradeStam;

    Block block;
    PlayerAnimationEvents animationEvents;
    PlayerMovement playerMovement;
    Coroutine regenCooldown;
    public bool CanRun { get; private set; } = true;
    bool canRegen = true;
    public bool IsExhausted { get => currentStamina <= 0; }
    public int Upgrade => upgradeStam;
    public float Value => maxStamina;

    public void StopRegen()
    {
        if (regenCooldown != null)
            StopCoroutine(regenCooldown);
        canRegen = false;
    }
    public void StartRegenAfterCooldown(float cooldown = .7f)
    {
        if (regenCooldown != null)
            StopCoroutine(regenCooldown);
        regenCooldown = StartCoroutine(RegenCooldown(cooldown));
    }
    IEnumerator RegenCooldown(float cooldown)
    {
        yield return new WaitForSeconds(cooldown);
        canRegen = true;
    }

    private void Awake()
    {
        playerStaminabar = GameObject.FindGameObjectWithTag("DisplayedStamina").GetComponent<DisplayBar>();
        block = GetComponent<Block>();
        animationEvents = GetComponentInChildren<PlayerAnimationEvents>();
        playerMovement = GetComponent<PlayerMovement>();
    }
    private void OnEnable()
    {
        ResetStamina();
    }
    private void Update()
    {
        if (IsExhausted && CanRun)
            StartCoroutine(ExhaustionCooldown());
        if (canRegen && currentStamina < maxStamina && !playerMovement.IsSprinting)
        {
            float staminaToRegen = ((block.IsBlocking ? blockingRegenMultiplier : 1) * staminaRegenRate * Time.deltaTime);
            currentStamina += staminaToRegen;
            playerStaminabar.Add(staminaToRegen, maxStamina);
            if (currentStamina > maxStamina)
                ResetStamina();
        }
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
            currentStamina -= value;
            if (currentStamina < 0)
                currentStamina = 0;
            playerStaminabar.Remove(value, maxStamina, false, syncLingeredValue);
        }
    }
    private IEnumerator ExhaustionCooldown()
    {
        CanRun = false;
        yield return new WaitUntil(() => currentStamina >= staminaRequiredToRun);
        CanRun = true;
    }

    public void Recharge(RechargeType rechargeType)
    {
        ResetStamina();
        playerStaminabar.Add(maxStamina, maxStamina);
    }

    public void UpgradeStat(int nbAmelioration)
    {
        maxStamina += upgradeStam * nbAmelioration;
    }
}
