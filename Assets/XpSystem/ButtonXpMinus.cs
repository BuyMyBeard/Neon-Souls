using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonXpMinus : MonoBehaviour
{
    MenuXp menuXp;
    AffichageXp typeStat;
    TextMeshProUGUI valeurAfficher;
    private void Awake()
    {
        menuXp = GetComponentInParent<MenuXp>();
        typeStat = GetComponentInParent<AffichageXp>();
        valeurAfficher = GetComponentInParent<TextMeshProUGUI>();
    }

    public void Minus()
    {
        menuXp.AddChanges(typeStat.statVisé, 5);
    }
}
