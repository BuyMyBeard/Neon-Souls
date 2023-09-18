using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    protected Slider trueHealth, displayedHealth;
    [SerializeField] protected TextMeshProUGUI damageValue;

    public float TrueValue
    {
        get => trueHealth.value;
        set => trueHealth.value = value;
    }
    public float DisplayedValue
    {
        get => displayedHealth.value;
        set => displayedHealth.value = value;
    }
    public string DamageValue
    {
        get => damageValue.text;
        set => damageValue.SetText(value);
    }
    public bool DisplayDamageValue
    {
        get => damageValue.gameObject.activeSelf;
        set => damageValue.gameObject.SetActive(value);
    }
    protected virtual void Awake()
    {   
        displayedHealth = GetComponent<Slider>();
        trueHealth = GetComponentInChildren<Slider>();
    }
}