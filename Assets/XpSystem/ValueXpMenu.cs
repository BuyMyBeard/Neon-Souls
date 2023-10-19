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

    [SerializeField] float ameliorateur = 0.10f;
    [SerializeField] TypeStat statType;

    public Type statVis� { get; private set; }

    public float Ameliorateur => ameliorateur;
    
    TextMeshProUGUI TextUi;

    private void Awake()
    {
        TextUi = GetComponent<TextMeshProUGUI>();
    }

    private void Start()
    {
        switch (statType)
        {
            case TypeStat.Health:
                statVis� = typeof(Health);
                break;
            case TypeStat.Stamina:
                statVis� = typeof(Stamina);
                break;
            case TypeStat.Attaque:
                statVis� = typeof(PlayerMeleeAttack);
                break;
            /*case TypeStat.Magie:
                statVis� = typeof(Magic);
                break;*/
            default:
                throw new Exception("Type de stat introuvable : AffichageXp");
        }
        var StatsPlayer = FindObjectOfType<PlayerExperience>().GetComponents<IStat>();
        foreach (IStat stat in StatsPlayer)
        {
            if(stat.GetType() == statVis�)
            {
                TextUi.text = stat.Value.ToString();
            }
        }
    }
}
