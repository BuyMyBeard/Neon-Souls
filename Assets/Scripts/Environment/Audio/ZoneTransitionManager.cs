using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Zone { Street, DanceClub, Garden, Mansion, MansionBackyard, Roof }
public class ZoneTransitionManager : MonoBehaviour
{
    ZoneExclusiveLoop[] zoneLoops;

    private void Start()
    {
        zoneLoops = FindObjectsByType<ZoneExclusiveLoop>(FindObjectsSortMode.InstanceID);
        Zone startZone = GameObject.FindGameObjectWithTag("StartingBonfire").GetComponent<Bonfire>().Zone;
        foreach (ZoneExclusiveLoop zone in zoneLoops)
        {
            if (zone.Zone != startZone)
            {
                zone.Stop();
            }
        }
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
