using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimationEvents : MonoBehaviour
{
    Interact interact;
    private void Awake()
    {
        interact = GetComponentInParent<Interact>();
    }
    void InteractEnd()
    {
        interact.EndInteract();
    }
}
