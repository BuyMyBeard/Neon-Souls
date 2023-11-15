using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Doors : MonoBehaviour
{
    Collider[] blockingColliders;
    private void Awake()
    {
        blockingColliders = GetComponentsInParent<Collider>();
    }

    private void OnTriggerEnter(Collider other)
    {
        foreach(Collider c in blockingColliders) { c.enabled = false; }
    }
}
