using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : Interactable
{
    Rigidbody[] rbs;
    public override string animationTriggerName { get; } = "Interact";
    [SerializeField] bool unlocked = false;
    protected override void Awake()
    {
        base.Awake();
        rbs = GetComponentsInChildren<Rigidbody>();
        promptMessage = "Ouvrir les portes";
        if (unlocked) Unlock();
    }
    public override void Interact()
    {
        base.Interact();
        Unlock();
    }
    void Unlock()
    {
        foreach (Rigidbody rb in rbs) { rb.isKinematic = false; }
        GetComponent<Collider>().enabled = false;
    }
}
