using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    
    List<IStat> upgratableStats = new();
    PlayerExperience playerXp;

    private void Start()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        upgratableStats.AddRange(playerXp.GetComponents<IStat>());
    }
    public void DistribuerXp(int xpAmount)
    {
        playerXp.GainXp(xpAmount);
    }
    public void UseXp(Type typeStatVisé,float upgradeValue)
    {
        foreach (IStat stat in upgratableStats)
        {
            if (stat.GetType() == typeStatVisé)
                stat.Ameliorateur += upgradeValue;
        }
    }
}
