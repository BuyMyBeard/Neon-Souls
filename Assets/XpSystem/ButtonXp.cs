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
    float defaultValue;
    private void Awake()
    {
        xpManager = FindObjectOfType<XpManager>();
        typeStat = GetComponentInParent<ValueXpMenu>();
        textAfficher = GetComponentInParent<TextMeshProUGUI>();
    }
    private IEnumerator Start()
    {
        yield return null;
        defaultValue = float.Parse(textAfficher.text.ToString(), System.Globalization.NumberStyles.Float);
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
            ChangeColor(Color.green);
        }
        //handle refusal of upgrade
    }
    void Minus()
    {
        if (xpManager.substractNbChanges(typeStat.statVisé))
        {
            valeurText -= typeStat.Ameliorateur;
            textAfficher.text = (valeurText).ToString();
            if(valeurText == defaultValue)
            {
                ChangeColor(Color.white);
            }
        }
        //handle refusal of downgrade
    }
    public void ChangeColor(Color color)
    {
        textAfficher.GetComponentInParent<TextMeshProUGUI>().color = color;
    }
    public void ResetDefault()
    {
        defaultValue = float.Parse(textAfficher.text.ToString(), System.Globalization.NumberStyles.Float);
    }
}
