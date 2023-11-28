using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogGateInteraction : Interactable
{
    bool disabled = false;
    public override string animationTriggerName => "WalkThroughFogGate";

    public override void Interact()
    {
        base.Interact();
        gameObject.layer = 6;
        foreach(Transform child in transform)
            child.gameObject.layer = 6;
        StartCoroutine(GivebackLayer());
    }
    IEnumerator GivebackLayer()
    {
        yield return new WaitForSeconds(3.12f);
        gameObject.layer = 0;
        foreach(Transform child in transform)
            child.gameObject.layer = 0;
    }

    public void Disable()
    {
        if (disabled) return;
        disabled = true;
        foreach (var collider in GetComponentsInChildren<Collider>())
            collider.enabled = false;

        GetComponentInChildren<ParticleSystem>().Stop();
        
    }
}
