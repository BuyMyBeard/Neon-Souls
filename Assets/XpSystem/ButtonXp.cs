using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ButtonXp : MonoBehaviour
{
    enum PlusOrMinus { Plus, Minus }
    [SerializeField] PlusOrMinus type;
    XpManager xpManager;
    ValueXpMenu typeStat;
    TextMeshProUGUI textAfficher;
    float valeurText;
    private void Awake()
    {
        xpManager = FindObjectOfType<XpManager>();
        typeStat = GetComponentInParent<ValueXpMenu>();
        textAfficher = GetComponentInParent<TextMeshProUGUI>();
        valeurText = float.Parse(textAfficher.text);
    }

    public void Uses() 
    {
        if (type == PlusOrMinus.Plus)
            Plus();
        else
            Minus();
    }

    void Plus()
    {
        xpManager.AddChanges(typeStat.statVisé, typeStat.Ameliorateur);
        valeurText += valeurText * typeStat.Ameliorateur;
        textAfficher.text = (valeurText).ToString();
    }
    void Minus()
    {
        xpManager.AddChanges(typeStat.statVisé, -typeStat.Ameliorateur);
        valeurText -= valeurText * typeStat.Ameliorateur;
        textAfficher.text = (valeurText).ToString();
    }
}
