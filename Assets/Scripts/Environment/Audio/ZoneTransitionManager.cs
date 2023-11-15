using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Zone { Street, DanceClub, Garden, Mansion, MansionBackyard, Roof }
public class ZoneTransitionManager : MonoBehaviour
{
    ZoneExclusiveLoop[] zoneLoops;

    private void Awake()
    {
        zoneLoops = FindObjectsByType<ZoneExclusiveLoop>(FindObjectsSortMode.InstanceID);
    }
    public void EnterZone(Zone zone)
    {
        foreach (ZoneExclusiveLoop zoneLoop in zoneLoops)
        {
            if (zoneLoop.Zone == zone)
                zoneLoop.StartFadeIn();
            else
                zoneLoop.StartFadeOut();
        }
    }
}
