using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class Spawner : MonoBehaviour, IRechargeable
{ 
    public abstract void Recharge(RechargeType rechargeType);
    [SerializeField] protected GameObject entity;
    [SerializeField] Color gizmosColor;
    [SerializeField] Mesh gizmosMesh;

    private void OnDrawGizmos()
    {
        Gizmos.color = gizmosColor;
        Gizmos.DrawMesh(gizmosMesh ,transform.position + Vector3.up, transform.rotation, transform.localScale);
    }
}
