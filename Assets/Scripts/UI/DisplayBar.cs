using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class DisplayBar : MonoBehaviour
{
    protected Slider trueHealth, displayedHealth;
    [SerializeField] protected TextMeshProUGUI damageValue;
    [SerializeField] float lingerTime = 1.5f;
    [SerializeField] float catchUpSpeed = 1;
    float timeSinceLastUpdated = 0;
    bool lingerTimerStarted = false;
    bool isCatchingUp = false;
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
        trueHealth = GetComponentInChildren<Slider>();

    }
    private void Update()
    {
        timeSinceLastUpdated += Time.deltaTime;
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
        lingerTimerCoroutine = LingerTimer();
        if (!lingerTimerStarted)
            StartCoroutine(lingerTimerCoroutine);
        timeSinceLastUpdated = 0;
        DisplayDamageValue = true;
        DamageValue = value.ToString();
    }

    IEnumerator LingerTimer()
    {
        lingerTimerStarted = true;
        yield return new WaitUntil(() => timeSinceLastUpdated > lingerTime);
        catchUpCoroutine = CatchUp(TrueValue);
        StartCoroutine(catchUpCoroutine);
        lingerTimerStarted = false;
        DisplayDamageValue = false;
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