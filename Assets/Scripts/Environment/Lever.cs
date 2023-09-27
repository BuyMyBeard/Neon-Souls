using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lever : Interactable
{
    
    public override void Interact()
    {
        base.Interact();
        Debug.Log("Levier IS PRESSED");
    }

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
