using System.Collections;
using System.Collections.Generic;
using UnityEditor.Search;
using UnityEngine;

public class Stamina : MonoBehaviour
{
    DisplayBar playerStaminabar;
    [SerializeField] int maxStamina = 100;
    [SerializeField] int currentStamina;
    [SerializeField] int dodgeStamina = 20;
    [SerializeField] int lightAttackStamina = 25;
    [SerializeField] int heavyAttackStamina = 40;
    private void Awake()
    {
        playerStaminabar = GameObject.FindGameObjectWithTag("DisplayedStamina").GetComponent<DisplayBar>();
    }
    private void OnEnable()
    {
        ResetStamina();
    }
    private void OnDodge()
    {
        if(currentStamina > 0)
        {
            currentStamina -= dodgeStamina;
            playerStaminabar.Remove(dodgeStamina, maxStamina, true);
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

}
