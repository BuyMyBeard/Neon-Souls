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
    float shownedValue;
    float defaultValue;
    private void Awake()
    {
        xpManager = FindObjectOfType<XpManager>();
        typeStat = GetComponentInParent<ValueXpMenu>();
        textAfficher = GetComponentInParent<TextMeshProUGUI>();
    }
    private void Start()
    {
        StartCoroutine(LateStart());
    }
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        ResetDefault();
        ResetShownedValue();
    }
    public void Uses() 
    {
        shownedValue = float.Parse(textAfficher.text.ToString(), System.Globalization.NumberStyles.Float);
        if (type == PlusOrMinus.Plus)
            Plus();
        else
            Minus();
    }
    void Plus()
    {
        if (xpManager.AddNbChanges(typeStat.TargetedStat))
        {
            shownedValue += typeStat.Ameliorateur;
            textAfficher.text = (shownedValue).ToString();
            ChangeColor(Color.green);
        }
        //handle refusal of upgrade
    }
    void Minus()
    {
        if (xpManager.substractNbChanges(typeStat.TargetedStat))
        {
            shownedValue -= typeStat.Ameliorateur;
            textAfficher.text = (shownedValue).ToString();
            if(shownedValue == defaultValue)
            {
                ChangeColor(Color.white);
            }
        }
        //handle refusal of downgrade
    }
    public void ChangeColor(Color color)
    {
        textAfficher.color = color;
    }
    public void ResetDefault()
    {
        defaultValue = float.Parse(textAfficher.text.ToString(), System.Globalization.NumberStyles.Float);
    }
    public void ResetShownedValue()
    {
        shownedValue = defaultValue;
        typeStat.Reset();
    }
}
