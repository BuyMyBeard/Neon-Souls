using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum Zone { Street, DanceClub, Garden, Mansion, MansionBackyard, Roof }
public class ZoneTransitionManager : MonoBehaviour
{
    [SerializeField] ZoneLoops[] zoneLoops;
    [Serializable] struct ZoneLoops
    {
        public Zone zone;
        public ZoneExclusiveLoop[] loops;
    }

    public void EnterZone(Zone zone)
    {
        foreach (ZoneLoops zoneLoops in zoneLoops)
        {
            if (zoneLoops.zone == zone)
                foreach (ZoneExclusiveLoop loop in zoneLoops.loops)
                    loop.StartFadeIn();

            else
                foreach(ZoneExclusiveLoop loop in zoneLoops.loops)
                    loop.StartFadeOut();
        }
    }
}
