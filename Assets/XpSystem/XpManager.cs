using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class XpManager : MonoBehaviour
{

    List<IStat> upgratableStats = new();
    PlayerExperience playerXp;
    Dictionary<IStat, int> DictioChangesStat = new();
    [SerializeField] int CostForUpgrade = 10;

    float localXpAmout;

    private void Start()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        upgratableStats.AddRange(playerXp.GetComponents<IStat>());

        
        foreach (IStat stat in upgratableStats)
        {
            DictioChangesStat.Add(stat, 0);
        }

        StartCoroutine(LateStart());
    }
    //just to make sure its after the start 
    IEnumerator LateStart()
    {
        yield return new WaitForSeconds(0.1f);
        localXpAmout = playerXp.XpAmount;
    }
    //ChangePlayer and stat
    public void DistribuerXp(int xpAmount)
    {
        playerXp.GainXp(xpAmount);
    }
    public void UseXp(IStat statVisé, int nbUpgrade)
    {
        statVisé.UpgradeStat(nbUpgrade);
        playerXp.removeXp(CostForUpgrade * nbUpgrade);
    }
    //LocalChange to verify integrity
    public bool AddNbChanges(IStat stat)
    {
        if (localXpAmout - CostForUpgrade >= 0)
        {
            localXpAmout -= CostForUpgrade;
            DictioChangesStat[stat] += 1;
            return true;
        }
        return false;
    }
    public bool substractNbChanges(IStat stat)
    {
        if (localXpAmout + CostForUpgrade <= playerXp.XpAmount && DictioChangesStat[stat] > 0)
        {
            localXpAmout += CostForUpgrade;
            DictioChangesStat[stat] -= 1;
            return true;
        }
        return false;
    }
    //Aplies changes to stat
    public void ValidateChanges()
    {
        foreach ((IStat stat, int value) in DictioChangesStat)
        {
            UseXp(stat, value);
            DictioChangesStat[stat] = 0;
        }
        localXpAmout = playerXp.XpAmount;
    }
}
