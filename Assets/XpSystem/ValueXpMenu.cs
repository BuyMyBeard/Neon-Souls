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
        Attaque/*,
        Magie*/
    }

    [SerializeField] TypeStat statType;

    PlayerExperience playerExperience;
    public IStat statVisé { get; private set; }

    public float Ameliorateur { get; private set; }
    
    TextMeshProUGUI TextUi;

    private void Awake()
    {
        TextUi = GetComponent<TextMeshProUGUI>();
        playerExperience = FindObjectOfType<PlayerExperience>();
    }

    private void Start()
    {
        switch (statType)
        {
            case TypeStat.Health:
                statVisé = playerExperience.GetComponent<Health>();
                break;
            case TypeStat.Stamina:
                statVisé = playerExperience.GetComponent<Stamina>();
                break;
            case TypeStat.Attaque:
                statVisé = playerExperience.GetComponent<PlayerMeleeAttack>();
                break;
            /*case TypeStat.Magie:
                statVisé = typeof(Magic);
                break;*/
            default:
                throw new Exception("Type de stat introuvable : AffichageXp");
        }
        
        TextUi.text = statVisé.Value.ToString();
        Ameliorateur = statVisé.Ameliorateur;
    }
}
