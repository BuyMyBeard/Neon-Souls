using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public abstract class Powerup : MonoBehaviour
{
    new Renderer renderer;
    new Collider collider;
    public bool IsVisibleAndTangible
    {
        get
        {
            return renderer.enabled && collider.enabled;
        }
        set
        {
            renderer.enabled = collider.enabled = value;
        }
    }

    protected GameObject player;

    // Awake is called before the first frame update
    protected virtual void Awake()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    [ContextMenu("Apply")]
    public virtual void Apply()
    {
        IsVisibleAndTangible = false;
    }

    protected virtual void OnTriggerEnter(Collider other)
    {
        Apply();
    }
}
