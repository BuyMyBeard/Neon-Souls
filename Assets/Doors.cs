using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Interactable
{
    Rigidbody[] rbs;
    public override string animationTriggerName { get; } = "Interact";
    
    protected override void Awake()
    {
        base.Awake();
        rbs = GetComponentsInChildren<Rigidbody>();
        promptMessage = "Ouvrir les portes";
    }
    public override void Interact()
    {
        base.Interact();
        foreach (Rigidbody rb in rbs) { rb.isKinematic = false; }
        GetComponent<Collider> ().enabled = false;
    }

}
