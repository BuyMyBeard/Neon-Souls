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
}
