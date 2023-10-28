using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class FallApart : MonoBehaviour
{
    [SerializeField] float density = 10.0f;
    MeshCollider[] parts;
    Animator animator;
    [SerializeField] Transform explosionSource;
    [SerializeField] float explosionStrength = 10f;
    [SerializeField] float explosionRadius = 10f;
    [SerializeField] float upwardsModifier = 2f;
    [SerializeField] Collider sword;
    private void Awake()
    {
        parts = GetComponentsInChildren<MeshCollider>();
        animator = GetComponent<Animator>();
    }

    [ContextMenu("Activate")]
    public void Activate()
    {
        animator.enabled = false;
        GameObject ragdoll = new GameObject("Ragdoll");
        ragdoll.AddComponent<DestroyOnRecharge>();
        foreach (MeshCollider part in parts)
        {
            DetachAndSetUp(part, ragdoll);
        }
        DetachAndSetUp(sword, ragdoll);
        Destroy(gameObject);
    }
    public void DetachAndSetUp(Collider part, GameObject ragdoll)
    {
        Transform parent = part.transform.parent;
        Transform wireframe = null;
        foreach (Transform child in parent)
        {
            if (child.name.Contains("Wireframe"))
            {
                wireframe = child;
                break;
            }
        }
        part.transform.parent = null;
        if (wireframe != null)
            wireframe.parent = part.transform;
        var rb = part.AddComponent<Rigidbody>();
        part.gameObject.layer = 0;
        // Approximation of mass with bounding volume
        // rb.mass = density * part.bounds.size.x * part.bounds.size.y * part.bounds.size.z;
        rb.SetDensity(density);
        part.isTrigger = false;
        part.transform.parent = ragdoll.transform;
        rb.AddExplosionForce(explosionStrength, explosionSource != null ? explosionSource.position : transform.position, explosionRadius, upwardsModifier);
    }
}



public class MissingGameObjectException : UnityException
{
    public MissingGameObjectException(string message) : base(message) { }
}
