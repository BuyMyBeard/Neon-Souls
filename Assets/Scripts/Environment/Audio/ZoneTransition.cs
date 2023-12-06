using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    ZoneTransitionManager zoneTransitionManager;
    [SerializeField] Zone enterZone;
    [SerializeField] Zone exitZone;
    private void Awake()
    {
        zoneTransitionManager = FindObjectOfType<ZoneTransitionManager>();
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawLine(transform.position, transform.position + transform.forward * 2);
        Gizmos.color = Color.red;
        Gizmos.DrawLine(transform.position, transform.position - transform.forward * 2);
    }

    private void OnTriggerExit(Collider other)
    {
        Vector3 directionToPlayer = other.transform.position - transform.position;
        directionToPlayer.y = 0;
        directionToPlayer = directionToPlayer.normalized;
        if (Vector3.Dot(directionToPlayer, transform.forward) > 0)
        {
            Debug.Log($"Entered {System.Enum.GetName(typeof(Zone), enterZone)}");
            zoneTransitionManager.EnterZone(enterZone);
        }
        else
        {
            Debug.Log("Exited");
            zoneTransitionManager.EnterZone(exitZone);
        }
    }
}