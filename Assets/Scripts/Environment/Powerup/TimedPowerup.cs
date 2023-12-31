using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class TimedPowerup : RechargeablePowerup
{
    [SerializeField] protected float duration;
    Coroutine revertAfterTime = null;
    protected IEnumerator RevertAfterTime(float time)
    {
        if (statusIconInstance != null)
        {
            Slider sliderInstance = statusIconInstance.GetComponent<Slider>();
            while (sliderInstance.value > 0.0)
            {
                yield return null;
                sliderInstance.value -= Time.deltaTime / duration;
            }
        }
        else
        {
            yield return new WaitForSeconds(duration);
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
    public override void Recharge(RechargeType rechargeType)
    {
        StopRevertAfterTime();
        base.Recharge(rechargeType);
    }

    public override void Apply()
    {
        base.Apply();
        revertAfterTime = StartCoroutine(RevertAfterTime(duration));
    }
    public override void Revert()
    {
        StopRevertAfterTime();
        base.Revert();
    }
}