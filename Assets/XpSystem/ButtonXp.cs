using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
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
    }
    public void Uses() 
    {
        valeurText = float.Parse(textAfficher.text.ToString(), System.Globalization.NumberStyles.Float);
        if (type == PlusOrMinus.Plus)
            Plus();
        else
            Minus();
    }

    void Plus()
    {
        if (xpManager.AddNbChanges(typeStat.statVisé))
        {
            valeurText += typeStat.Ameliorateur;
            textAfficher.text = (valeurText).ToString();
        }
        //handle refusal of upgrade
    }
    void Minus()
    {
        if (xpManager.substractNbChanges(typeStat.statVisé))
        {
            valeurText -= typeStat.Ameliorateur;
            textAfficher.text = (valeurText).ToString();
        }
        //handle refusal of downgrade
    }
}
