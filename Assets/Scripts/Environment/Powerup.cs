using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class Powerup : MonoBehaviour, IRechargeable
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
    protected virtual bool IsTemporary => false;

    Coroutine revertAfterTime = null;

    // Awake is called before the first frame update
    protected virtual void Awake()
    {
        renderer = GetComponent<Renderer>();
        collider = GetComponent<Collider>();
        player = GameObject.FindGameObjectWithTag("Player");
    }

    protected void SetRevertTimer(float time)
    {
        revertAfterTime = StartCoroutine(RevertAfterTime(time));
    }

    [ContextMenu("Apply")]
    public abstract void Apply();

    [ContextMenu("Revert")]
    public abstract void Revert();

    protected IEnumerator RevertAfterTime(float time)
    {
        yield return new WaitForSeconds(time);
        Revert();
        revertAfterTime = null;
    }

    private void OnTriggerEnter(Collider other)
    {
        Apply();
        IsVisibleAndTangible = false;
    }
    public void Recharge()
    {
        if (IsTemporary)
        {
            IsVisibleAndTangible = true;
            Revert();
            if (revertAfterTime != null)
            {
                revertAfterTime = null;
                StopCoroutine(revertAfterTime);
            }
        }
    }
}
