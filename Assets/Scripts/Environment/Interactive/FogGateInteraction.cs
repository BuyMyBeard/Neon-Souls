using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogGateInteraction : Interactable
{
    bool disabled = false;
    public override string animationTriggerName => "WalkThroughFogGate";
    Collider[] colliders;

    protected override void Awake()
    {
        base.Awake();
        colliders = GetComponentsInChildren<Collider>();
    }

    public override void Interact()
    {
        base.Interact();
        foreach (Collider collider in colliders)
            collider.enabled = false;
        StartCoroutine(ReEnableColliders());
    }
    IEnumerator ReEnableColliders()
    {
        yield return new WaitForSeconds(3.12f);
        foreach (Collider collider in colliders)
            collider.enabled = true;
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
