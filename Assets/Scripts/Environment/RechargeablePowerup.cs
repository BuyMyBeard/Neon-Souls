using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class RechargeablePowerup : Powerup, IRechargeable
{
    protected bool isActive = false;
    [SerializeField] AnimationCurveScriptable curve;
    [SerializeField] float statusIconDisappearTime;
    IEnumerator StatusEnd()
    {
        var currentStatusIcon = statusIconInstance;
        Slider sliderInstance = currentStatusIcon.GetComponent<Slider>();
        float elapsedTime = 0.0f;
        float original = sliderInstance.value;
        while (sliderInstance.value > 0.0)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            sliderInstance.value = curve.Evaluate(elapsedTime) * original;
        }

        Image image = currentStatusIcon.GetComponentInChildren<Image>();
        Color a = image.color;
        Color b = image.color;
        b.a = 0;

        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / statusIconDisappearTime)
        {
            yield return null;
            image.color = Color.Lerp(a, b, t);
        }

        Destroy(currentStatusIcon);
        if (statusIconInstance == currentStatusIcon)
            statusIconInstance = null;
    }

    public virtual void Recharge()
    {
        IsVisibleAndTangible = true;
        if (isActive)
        {
            Revert();
        }
    }
    public override void Apply()
    {
        base.Apply();
        isActive = true;
    }
    public virtual void Revert()
    {
        StartCoroutine(StatusEnd()); // This calls base.Revert
        isActive = false;
    }
}