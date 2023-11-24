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
    protected Slider trueValueSlider, lingeredValueSlider;
    [SerializeField] protected TextMeshProUGUI damageValue;
    [SerializeField] float lingerTime = 0.5f;
    [SerializeField] float damageValueLingerTime = 2f;
    [SerializeField] float catchUpSpeed = 2f;
    float displayBarTimer = 0;
    float damageValueTimer = 0;
    bool lingerTimerStarted = false;
    bool isCatchingUp = false;
    public bool Hidden { get; protected set; } = false;
    float stackedValue = 0;
    IEnumerator lingerTimerCoroutine, catchUpCoroutine;

    /// <summary>
    /// Slider value displayed. Goes from 0 to 1
    /// </summary>
    protected float TrueValue
    {
        get => trueValueSlider.value;
        set => trueValueSlider.value = value;
    }
    /// <summary>
    /// Slider value displayed. Lags behind TrueValue when dropping. Goes from 0 to 1
    /// </summary>
    protected float LingeredValue
    {
        get => lingeredValueSlider.value;
        set => lingeredValueSlider.value = value;
    }
    /// <summary>
    /// Stacked value displayed near slider to show stacked damage or debugging value
    /// </summary>
    protected string DamageValue
    {
        get => damageValue.text;
        set => damageValue.SetText(value);
    }
    /// <summary>
    /// If true, Stacked value is shown
    /// </summary>
    protected bool ShowDamageValue
    {
        get => damageValue.gameObject.activeSelf;
        set => damageValue.gameObject.SetActive(value);
    }
    protected virtual void Awake()
    {
        lingeredValueSlider = GetComponent<Slider>();
        trueValueSlider = GetComponentsInChildren<Slider>()[1];
    }
    private void Update()
    {
        displayBarTimer += Time.deltaTime;
        damageValueTimer += Time.deltaTime;
    }
    private void OnEnable()
    {
        ShowDamageValue = false;
    }

    /// <summary>
    /// Adds a value to the display bar
    /// </summary>
    /// <param name="value">Value added</param>
    /// <param name="max">Value the display bar would take if it was full</param>
    public void Add(float value, float max, bool shouldLinger = true)
    {
        TrueValue += value / max;

        if (!shouldLinger) return;

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
        if (TrueValue > LingeredValue)
            LingeredValue = TrueValue;
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
    public void Remove(float value, float max, bool showValue, bool syncLingeredValue = false)
    {
        TrueValue -= value / max;
        displayBarTimer = 0;
        damageValueTimer = 0;
        if (syncLingeredValue)
        {
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
            LingeredValue = TrueValue;
        }
        else
        {
            lingerTimerCoroutine = LingerTimer();
            if (!lingerTimerStarted)
                StartCoroutine(lingerTimerCoroutine);
        }
        if (showValue && !Hidden)
        {
            stackedValue += value;
            DamageValue = Mathf.RoundToInt(stackedValue).ToString();
            if (!ShowDamageValue)
                StartCoroutine(DamageDisplayTimer());
        }
    }
    public void Set(float value, float max)
    {
        TrueValue = value / max;
        LingeredValue = value / max;
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
    }

    public virtual void Show(int damage = 0)
    {
        if (!Hidden) return;
        Hidden = false;
        foreach (Transform child in transform)
            child.gameObject.SetActive(true);
        
        //setting active all children recursively also shows stacking damage value, so it's manually hidden after
        ShowDamageValue = false;
    }

    public virtual void Hide()
    {
        Hidden = true;
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

        foreach(Transform child in transform)
            child.gameObject.SetActive(false);
    }

    IEnumerator DamageDisplayTimer()
    {
        ShowDamageValue = true;
        yield return new WaitUntil(() => damageValueTimer > damageValueLingerTime);
        ShowDamageValue = false;
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
        float start = LingeredValue;
        for (float t = start; t >= goal; t -= Time.deltaTime * catchUpSpeed)
        {
            yield return null;
            LingeredValue = t;
        }
        LingeredValue = goal;
        isCatchingUp = false;
    }
}