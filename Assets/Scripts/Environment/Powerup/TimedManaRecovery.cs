using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TimedManaRecovery : TimedPowerup
{
    Mana mana;
    [SerializeField] float totalManasRecovered;

    IEnumerator RecoverMana()
    {
        float manasPerSecond = totalManasRecovered / duration;
        for (float t = 0; t < duration; t += Time.deltaTime)
        {
            mana.Add(Time.deltaTime * manasPerSecond, false);
            yield return null;
        }
    }

    protected override void Awake()
    {
        base.Awake();
        mana = player.GetComponentInParent<Mana>();
    }
    public override void Apply()
    {
        base.Apply();
        StartCoroutine(nameof(RecoverMana));
    }
    public override void Recharge(RechargeType rechargeType)
    {
        base.Recharge(rechargeType);
        StopCoroutine(nameof(RecoverMana));
    }
}
