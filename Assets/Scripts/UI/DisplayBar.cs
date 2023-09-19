using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

/// <summary>
/// Class reprensenting Health bars and Stamina bars. Manages the values of Sliders linked to it.
/// </summary>
public class DisplayBar : MonoBehaviour
{
    protected Slider trueHealth, displayedHealth;
    [SerializeField] protected TextMeshProUGUI damageValue;
    [SerializeField] float lingerTime = 0.5f;
    [SerializeField] float damageValueLingerTime = 2f;
    [SerializeField] float catchUpSpeed = 2f;
    float displayBarTimer = 0;
    float damageValueTimer = 0;
    bool lingerTimerStarted = false;
    bool isCatchingUp = false;
    float stackedValue = 0;
    IEnumerator lingerTimerCoroutine;
    IEnumerator catchUpCoroutine;

    protected float TrueValue
    {
        get => trueHealth.value;
        set => trueHealth.value = value;
    }
    protected float DisplayedValue
    {
        get => displayedHealth.value;
        set => displayedHealth.value = value;
    }
    protected string DamageValue
    {
        get => damageValue.text;
        set => damageValue.SetText(value);
    }
    protected bool DisplayDamageValue
    {
        get => damageValue.gameObject.activeSelf;
        set => damageValue.gameObject.SetActive(value);
    }
    protected virtual void Awake()
    {   
        // Would be more logical to set up as SerializeField for ease of use
        displayedHealth = GetComponent<Slider>();
        trueHealth = GetComponentsInChildren<Slider>()[1];
        
    }
    private void Update()
    {
        displayBarTimer += Time.deltaTime;
        damageValueTimer += Time.deltaTime;
    }
    private void OnEnable()
    {
        DisplayDamageValue = false;
    }

    /// <summary>
    /// Adds a value to the display bar
    /// </summary>
    /// <param name="value">Value added</param>
    /// <param name="max">Value the display bar would take if it was full</param>
    public void Add(int value, int max)
    {
        TrueValue += value / max;

        if (lingerTimerStarted)
        {
            StopCoroutine(lingerTimerCoroutine);
            lingerTimerStarted = false;
        }
        if (isCatchingUp)
        {
            StopCoroutine(catchUpCoroutine);
            isCatchingUp = false;
        }
        if (TrueValue > DisplayedValue)
            DisplayedValue = TrueValue;
        else
        {
            catchUpCoroutine = CatchUp(TrueValue);
            StartCoroutine(catchUpCoroutine);
        }
    }
    /// <summary>
    /// Removes a value from the display bar
    /// </summary>
    /// <param name="value">Value removed</param>
    /// <param name="max">Value the display bar would take if it was full</param>
    /// <param name="showValue">If true, stacked value is displayed near the display bar</param>
    public void Remove(int value, int max, bool showValue)
    {
        TrueValue -= value / max;
        displayBarTimer = 0;
        damageValueTimer = 0;
        lingerTimerCoroutine = LingerTimer();
        if (!lingerTimerStarted)
            StartCoroutine(lingerTimerCoroutine);
        if (showValue)
        {
            stackedValue += value;
            DamageValue = stackedValue.ToString();
            if (!DisplayDamageValue)
                StartCoroutine(DamageDisplayTimer());
        }
    }

    IEnumerator DamageDisplayTimer()
    {
        DisplayDamageValue = true;
        yield return new WaitUntil(() => damageValueTimer > damageValueLingerTime);
        DisplayDamageValue = false;
        stackedValue = 0;
    }

    IEnumerator LingerTimer()
    {
        lingerTimerStarted = true;
        yield return new WaitUntil(() => displayBarTimer > lingerTime);
        catchUpCoroutine = CatchUp(TrueValue);
        StartCoroutine(catchUpCoroutine);
        lingerTimerStarted = false;
    }

    IEnumerator CatchUp(float goal)
    {
        isCatchingUp = true;
        float start = DisplayedValue;
        for (float t = 0; t <= 1; t += Time.deltaTime * catchUpSpeed)
        {
            yield return null;
            DisplayedValue = Mathf.Lerp(start, goal, t);
        }
        DisplayedValue = goal;
        isCatchingUp = false;
    }
}