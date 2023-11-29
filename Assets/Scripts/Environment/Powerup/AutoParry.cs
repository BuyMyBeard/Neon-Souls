using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class AutoParry : RechargeablePowerup
{
    PlayerHealth playerHealth;
    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.GetComponentInParent<PlayerHealth>();
    }
    public override void Apply()
    {
        base.Apply();
        playerHealth.isAutoParryOn = true;
        playerHealth.OnHit.AddListener(Revert);
    }
    public override void Revert()
    {
        base.Revert();
        playerHealth.isAutoParryOn = false;
        // TODO: Find out if this removes all occurences of Powerup.Revert or BlockIsParry.Revert
        playerHealth.OnHit.RemoveListener(Revert);
    }
}
