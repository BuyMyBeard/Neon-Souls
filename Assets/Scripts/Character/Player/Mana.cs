using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mana : MonoBehaviour, IRechargeable
{
    DisplayBar playerManabar;
    [SerializeField] int maxMana = 100;
    [SerializeField] float currentMana;
    public bool IsExhausted { get => currentMana <= 0; }

    private void Awake()
    {
        playerManabar = GameObject.FindGameObjectWithTag("DisplayedMana").GetComponent<DisplayBar>();
    }
    private void OnEnable()
    {
        ResetMana();
    }
    public bool CanCast(int manaCost) => currentMana >= manaCost;
    private void ResetMana()
    {
        currentMana = maxMana;
    }
    /// <summary>
    /// Remove a defined amount of mana
    /// </summary>
    /// <param name="value">Amount to remove</param>
    public void Remove(float value)
    {
        if (!IsExhausted)
        {
            currentMana -= value;
            if (currentMana < 0)
                currentMana = 0;
            playerManabar.Remove(value, maxMana, true, false);
        }
    }
    public void Recharge()
    {
        ResetMana();
        playerManabar.Add(maxMana, maxMana);
    }
}
