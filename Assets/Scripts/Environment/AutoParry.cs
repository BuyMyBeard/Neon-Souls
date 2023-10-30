using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class AutoParry : Powerup
{
    protected override bool IsTemporary => true;
    PlayerHealth playerHealth;
    protected override void Awake()
    {
        base.Awake();
        playerHealth = player.GetComponentInParent<PlayerHealth>();
    }
    public override void Apply()
    {
        playerHealth.isAutoParryOn = true;
        playerHealth.OnHit.AddListener(Revert);
    }

    public override void Revert()
    {
        playerHealth.isAutoParryOn = false;
        // TODO: Find out if this removes all occurences of Powerup.Revert or BlockIsParry.Revert
        playerHealth.OnHit.RemoveListener(Revert);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
