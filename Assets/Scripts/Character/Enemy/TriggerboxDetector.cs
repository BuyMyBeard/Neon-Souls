using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
public class TriggerboxDetector : MonoBehaviour, IRechargeable
{
    [SerializeField] Enemy[] enemies;
    [SerializeField] Enemy.ModeId modeToChangeTo = Enemy.ModeId.InRange;
    [SerializeField] UnityEvent OnTrigger = new();
    bool alreadyTriggered = false;

    private void Awake()
    {
        Collider collider = GetComponent<Collider>();
        collider.enabled = true;
        collider.isTrigger = true;
        gameObject.layer = 22;
        if (TryGetComponent(out Renderer renderer)) renderer.enabled = false;
        if (enemies.Length == 0) throw new MissingReferenceException("Enemy has not been assigned to triggerbox detector");
    }


    private void OnTriggerEnter(Collider other)
    {
        if (alreadyTriggered) return;
        foreach (Enemy enemy in enemies) enemy.ChangeMode(modeToChangeTo);
        OnTrigger.Invoke();
        alreadyTriggered = true;
    }

    public void Recharge()
    {
        alreadyTriggered = false;
    }
}
