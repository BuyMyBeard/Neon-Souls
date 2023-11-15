using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ZoneTransition : MonoBehaviour
{
    [SerializeField] ZoneExclusiveLoop[] zone1Exclusive;
    [SerializeField] ZoneExclusiveLoop[] zone2Exclusive;
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
            EnterZone2();
        else
            EnterZone1();
    }

    public void EnterZone1()
    {
        foreach (ZoneExclusiveLoop loop in zone1Exclusive)
            loop.StartFadeOut();
        foreach (ZoneExclusiveLoop loop in zone2Exclusive)
            loop.StartFadeIn();
    }

    public void EnterZone2() 
    {
        foreach (ZoneExclusiveLoop loop in zone1Exclusive)
            loop.StartFadeIn();
        foreach (ZoneExclusiveLoop loop in zone2Exclusive)
            loop.StartFadeOut();
    }
}