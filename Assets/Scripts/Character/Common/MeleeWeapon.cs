using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MeleeWeapon : MonoBehaviour
{
    [HideInInspector] public UnityEvent<Collider> onTrigger;
    [HideInInspector] public int damage;
    [HideInInspector] public int staminaBlockCost;
    new Collider collider;
    readonly List<Health> opponentsHit = new();
    Transform user;
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
        user = GetComponentInParent<Health>().transform;
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
            opponentHealth.InflictBlockableDamage(damage, staminaBlockCost, user);
        }
    }
}