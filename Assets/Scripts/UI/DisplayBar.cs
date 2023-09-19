using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

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

    // TODO: Have yet to test any of this
    public void Add(float value, float max)
    {
        TrueValue += value / max;
        if (lingerTimerStarted)
            StopCoroutine(lingerTimerCoroutine);
        if (isCatchingUp)
            StartCoroutine(UpdateWhenDoneCatchingUp());
    }
    public void Remove(float value, float max)
    {
        TrueValue -= value / max;
        displayBarTimer = 0;
        damageValueTimer = 0;
        lingerTimerCoroutine = LingerTimer();
        if (!lingerTimerStarted)
            StartCoroutine(lingerTimerCoroutine);
        stackedValue += value;
        DamageValue = stackedValue.ToString();
        if (!DisplayDamageValue)
            StartCoroutine(DamageDisplayTimer());
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
    IEnumerator UpdateWhenDoneCatchingUp()
    {
        yield return new WaitUntil(() => !isCatchingUp);
        DisplayedValue = TrueValue;
    }
}