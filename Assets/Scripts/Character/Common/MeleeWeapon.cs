using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MeleeWeapon : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Collider> onTrigger;
    public int damage;
    new Collider collider;
    readonly List<Health> opponentsHit = new();
    public bool ColliderEnabled
    {
        get => collider.enabled;
        set
        {
            collider.enabled = value;
            opponentsHit.Clear();
        }
    }
    void Awake()
    {
        collider = GetComponent<Collider>();
        ColliderEnabled = false;
        collider.isTrigger = true;
    }
    private void Start()
    {
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void OnTriggerEnter(Collider other)
    {
        Health opponentHealth = other.GetComponentInParent<Health>();
        if (opponentHealth == null)
            throw new MissingComponentException("Character has missing Health component");
        // To avoid hitting the same enemy multiple times with the same attack
        else if (!opponentsHit.Contains(opponentHealth))
        {
            opponentsHit.Add(opponentHealth);
            opponentHealth.InflictDamage(damage);
        }
    }
}