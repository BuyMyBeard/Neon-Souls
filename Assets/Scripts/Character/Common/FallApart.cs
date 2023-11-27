using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FallApart : MonoBehaviour, IRechargeable
{
    [SerializeField] float density = 10.0f;
    Collider[] parts;
    Animator animator;
    GameObject model;
    GameObject character;
    [SerializeField] Transform explosionSource;
    //[SerializeField] float explosionStrength = 10f;
    //[SerializeField] float explosionRadius = 10f;
    //[SerializeField] float upwardsModifier = 2f;
    [SerializeField] int partsLayer = 0;
    [SerializeField] GameObject droppedXp;
    MeleeWeapon[] meleeWeapons;
    Sounds sounds;

    private void Awake()
    {
        animator = GetComponentInChildren<Animator>();
        character = animator.gameObject;
        model = character.transform.GetChild(0).gameObject;
        parts = model.GetComponentsInChildren<Collider>();
        meleeWeapons = GetComponentsInChildren<MeleeWeapon>();
        sounds = GetComponentInParent<Sounds>();
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        animator.enabled = false;
        GameObject ragdoll = new("Ragdoll");
        ragdoll.AddComponent<DestroyOnRecharge>();
        foreach (Collider part in parts)
        {
            if (part.gameObject.layer == 20) continue;
            DetachAndSetUp(part, ragdoll);
        }
        Destroy(gameObject);
    }
    void DetachAndSetUp(Collider part, GameObject ragdoll)
    {
        if (part.TryGetComponent(out MeleeWeapon mw))
        {
            if (mw.deathBehaviour == MeleeWeapon.DeathBehaviour.DetachEarly) 
                Destroy(mw); 
            else
            {
                Destroy(mw.gameObject);
                return;
            }
        }
        Transform parent = part.transform.parent;
        Transform wireframe = null;
        if (parent != null)
        {
            foreach (Transform child in parent)
            {
                if (child.name.Contains("Wireframe"))
                {
                    wireframe = child;
                    break;
                }
            }
        }
        part.transform.parent = null;
        if (wireframe != null)
            wireframe.parent = part.transform;
        if (!part.TryGetComponent<Rigidbody>(out var rb))
        {
            rb = part.AddComponent<Rigidbody>();
            rb.useGravity = true;
        }
        part.gameObject.layer = partsLayer;
        part.enabled = true;
        part.isTrigger = false;
        // Approximation of mass with bounding volume
        // rb.mass = density * part.bounds.size.x * part.bounds.size.y * part.bounds.size.z;
        rb.SetDensity(density);
        rb.isKinematic = false;
        rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        part.transform.parent = ragdoll.transform;
        // rb.AddExplosionForce(explosionStrength, explosionSource != null ? explosionSource.position : transform.position, explosionRadius, upwardsModifier);
    }

    [ContextMenu("Decompose")]
    public void Decompose()
    {
        GameObject dummy = Instantiate(character);
        dummy.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
        if (TryGetComponent(out Enemy _)) gameObject.SetActive(false);
        else model.SetActive(false);
        dummy.GetComponent<FallApart>().Activate();
        if (droppedXp == null) return;
        sounds.Play(Sound.Died, .25f);
        DroppedXp previousDroppedXp = FindObjectOfType<DroppedXp>();
        if(previousDroppedXp != null ) { Destroy(previousDroppedXp.gameObject); }
        Instantiate(droppedXp, character.transform.position, Quaternion.identity);
    }

    public void Recharge(RechargeType rechargeType)
    {
        if (CompareTag("Player")) model.SetActive(true);
        ReenableWeapons();
    }

    public void DetachWeapons()
    {
        foreach (var weapon in meleeWeapons)
        {
            if (weapon.deathBehaviour == MeleeWeapon.DeathBehaviour.Destroy) continue;
            MeleeWeapon ragdollWeapon = Instantiate(weapon, weapon.transform.position, weapon.transform.rotation, null);
            ragdollWeapon.transform.localScale = weapon.transform.lossyScale;
            Rigidbody rb = ragdollWeapon.GetComponent<Rigidbody>();
            weapon.gameObject.SetActive(false);
            ragdollWeapon.gameObject.layer = 16;
            Collider collider = ragdollWeapon.GetComponent<Collider>();
            ragdollWeapon.gameObject.AddComponent<DestroyOnRecharge>();
            ragdollWeapon.AddComponent<MeshCollider>().convex = true;
            Destroy(collider);
            Destroy(ragdollWeapon);
            rb.SetDensity(1);
            rb.isKinematic = false;
            rb.useGravity = true;
            rb.collisionDetectionMode = CollisionDetectionMode.Continuous;
        }
    }
    private void ReenableWeapons()
    {
        foreach (var weapon in meleeWeapons)
            weapon.gameObject.SetActive(true);
    }
}



public class MissingGameObjectException : UnityException
{
    public MissingGameObjectException(string message) : base(message) { }
}
