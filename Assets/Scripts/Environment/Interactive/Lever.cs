using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    public override string animationTriggerName => "Interact";

    public override void Interact()
    {
        base.Interact();
        Debug.Log("Levier IS PRESSED");
    }
}
