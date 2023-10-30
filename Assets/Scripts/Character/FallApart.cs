using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FallApart : MonoBehaviour
{
    [SerializeField] float density = 10.0f;
    Collider[] parts;
    Animator animator;
    [SerializeField] Transform explosionSource;
    [SerializeField] float explosionStrength = 10f;
    [SerializeField] float explosionRadius = 10f;
    [SerializeField] float upwardsModifier = 2f;
    [SerializeField] int partsLayer = 0;
    private void Awake()
    {
        parts = GetComponentsInChildren<Collider>();
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        animator.enabled = false;
        GameObject ragdoll = new GameObject("Ragdoll");
        ragdoll.AddComponent<DestroyOnRecharge>();
        foreach (Collider part in parts)
        {
            DetachAndSetUp(part, ragdoll);
        }
        Destroy(gameObject);
    }
    void DetachAndSetUp(Collider part, GameObject ragdoll)
    {
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
        Rigidbody rb = part.GetComponent<Rigidbody>();
        if (rb == null)
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
        var mw = part.GetComponent<MeleeWeapon>();
        if (mw != null) Destroy(mw); 
        part.transform.parent = ragdoll.transform;
        rb.AddExplosionForce(explosionStrength, explosionSource != null ? explosionSource.position : transform.position, explosionRadius, upwardsModifier);
    }
}



public class MissingGameObjectException : UnityException
{
    public MissingGameObjectException(string message) : base(message) { }
}
