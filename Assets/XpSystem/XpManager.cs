using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XpManager : MonoBehaviour
{
    
    List<IStat> upgratableStats = new();
    PlayerExperience playerXp;
    Dictionary<Type, float> DictioChangesStat = new();

    private void Start()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        upgratableStats.AddRange(playerXp.GetComponents<IStat>());

        foreach(IStat stat in upgratableStats)
        {
            DictioChangesStat.Add(stat.GetType(), 1);
        }
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
    public void AddChanges(Type type, float ameliorateur)
    {
        DictioChangesStat[type] += ameliorateur;
    }
    public void ValidateChanges()
    {
        foreach ((Type type, float value) in DictioChangesStat)
        {
            UseXp(type, value);
        }
    }
}
