using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class TimedPowerup : RechargeablePowerup
{
    [SerializeField] int cooldown;
    Coroutine revertAfterTime = null;
    protected IEnumerator RevertAfterTime(float time)
    {
        if (statusIconInstance != null)
        {
            Slider sliderInstance = statusIconInstance.GetComponent<Slider>();
            while (sliderInstance.value > 0.0)
            {
                yield return null;
                sliderInstance.value -= Time.deltaTime / cooldown;
            }
        }
        else
        {
            yield return new WaitForSeconds(cooldown);
        }
        Revert();
        revertAfterTime = null;
    }
    void StopRevertAfterTime()
    {
        if (revertAfterTime != null)
        {
            StopCoroutine(revertAfterTime);
            revertAfterTime = null;
        }
    }
    public override void Recharge()
    {
        StopRevertAfterTime();
        base.Recharge();
    }

    public override void Apply()
    {
        base.Apply();
        revertAfterTime = StartCoroutine(RevertAfterTime(cooldown));
    }
    public override void Revert()
    {
        StopRevertAfterTime();
        base.Revert();
    }
}