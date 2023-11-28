using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UpdateMirror : MonoBehaviour
{
    ReflectionProbe probe;
    private void Awake()
    {
        probe = GetComponentInChildren<ReflectionProbe>();
    }

    void OnTriggerStay()
    {
        probe.RenderProbe();
    }
}
