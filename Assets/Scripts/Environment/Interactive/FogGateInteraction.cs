using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FogGateInteraction : Interactable
{
    Animator playerAnimator;

    public override string animationTriggerName => "WalkThroughFogGate";

    public override void Interact()
    {
        base.Interact();
        gameObject.layer = 6;
        foreach(Transform child in transform)
            child.gameObject.layer = 6;
        StartCoroutine(GivebackLayer());
        Debug.Log("Fog");
    }
    IEnumerator GivebackLayer()
    {
        yield return new WaitForSeconds(3.12f);
        gameObject.layer = 0;
        foreach(Transform child in transform)
            child.gameObject.layer = 0;
    }
}
