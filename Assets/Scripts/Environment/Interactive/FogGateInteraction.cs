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
    }
}
