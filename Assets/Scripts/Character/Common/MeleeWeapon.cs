using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class MeleeWeapon : MonoBehaviour
{
    public enum DeathBehaviour { Destroy, DetachEarly }
    [SerializeField] BlockSound blockSound = BlockSound.SwordClash;
    [HideInInspector] public bool blockable = false;
    [HideInInspector] public UnityEvent<Collider> onTrigger;
    [HideInInspector] public int damage;
    [HideInInspector] public int staminaBlockCost;
    public DeathBehaviour deathBehaviour;
    new Collider collider;
    readonly List<Health> opponentsHit = new();
    Transform user = null;
    TrailRenderer trailRenderer;
    public bool ColliderEnabled
    {
        get => collider.enabled;
        set
        {
            if (trailRenderer != null) trailRenderer.emitting = value;
            collider.enabled = value;
            opponentsHit.Clear();
        }
    }
    void Awake()
    {
        trailRenderer = GetComponentInChildren<TrailRenderer>();
        collider = GetComponent<Collider>();
        ColliderEnabled = false;
        collider.isTrigger = true;
        var health = GetComponentInParent<Health>();
        if (health != null)
            user = health.transform;
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
            if (blockable)
                opponentHealth.InflictBlockableDamage(damage, staminaBlockCost, user, blockSound);
            else
                opponentHealth.InflictDamage(damage, user);
        }
    }
}