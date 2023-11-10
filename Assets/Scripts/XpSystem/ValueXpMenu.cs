using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(TextMeshProUGUI))]
public class ValueXpMenu : MonoBehaviour
{
    enum TypeStat
    {
        Health,
        Stamina,
        Attaque,
        Magie,
        AttaqueMagie
    }

    [SerializeField] TypeStat statType;

    PlayerExperience playerExperience;
    public IStat TargetedStat { get; private set; }

    public float Ameliorateur { get; private set; }
    
    TextMeshProUGUI TextUi;

    private void Awake()
    {
        TextUi = GetComponent<TextMeshProUGUI>();
        playerExperience = FindObjectOfType<PlayerExperience>();
    }

    private void Start()
    {
        TargetedStat = statType switch
        {
            TypeStat.Health => playerExperience.GetComponent<PlayerHealth>(),
            TypeStat.Stamina => playerExperience.GetComponent<Stamina>(),
            TypeStat.Attaque => playerExperience.GetComponent<PlayerMeleeAttack>(),
            TypeStat.Magie => playerExperience.GetComponent<Mana>(),
            TypeStat.AttaqueMagie => playerExperience.GetComponent<Spells>(),
            _ => throw new Exception("Type de stat introuvable : AffichageXp"),
        };
        ResetValue();
        Ameliorateur = TargetedStat.Upgrade;
    }
    public void ResetValue()
    {
        if(TextUi != null && TargetedStat != null)
            TextUi.text = TargetedStat.Value.ToString();
    }
}
