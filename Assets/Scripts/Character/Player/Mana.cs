using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.Rendering;

public class Mana : MonoBehaviour, IRechargeable,IStat
{
    DisplayBar playerManabar;
    [SerializeField] int maxMana = 100;
    [SerializeField] float currentMana;
    [SerializeField] int upgradeMana;
    public bool IsExhausted { get => currentMana <= 0; }

    public float Value => maxMana;

    public int Upgrade => upgradeMana;

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
    public void Add(float value, bool shouldLinger = true)
    {
        currentMana += value;
        if (currentMana > maxMana)
            currentMana = maxMana;
        playerManabar.Add(value, maxMana, shouldLinger);
    }
    public void Recharge()
    {
        ResetMana();
        playerManabar.Add(maxMana, maxMana);
    }

    public void UpgradeStat(int nbAmelioration)
    {
        maxMana += upgradeMana * nbAmelioration;
    }
}
