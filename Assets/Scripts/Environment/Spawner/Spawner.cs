using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour, IRechargeable
{ 
    public abstract void Recharge();
    [SerializeField] protected GameObject entity;
    [SerializeField] Color gizmosColor;
    [SerializeField] Mesh gizmosMesh;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawMesh(gizmosMesh ,transform.position, transform.rotation, transform.localScale);
    }
}
