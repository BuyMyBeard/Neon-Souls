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
    XpAmountText xpAmountText;

    [SerializeField] int CostForUpgrade = 10;


    float localXpAmount;
    private void Awake()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        upgratableStats.AddRange(playerXp.GetComponents<IStat>());
        xpAmountText = FindObjectOfType<XpAmountText>();    
    }

    private IEnumerator Start()
    {
        foreach (IStat stat in upgratableStats)
        {
            DictioChangesStat[stat] = 0;
        }
        yield return null;
        localXpAmount = playerXp.XpAmount;
    }
    //ChangePlayer and stat
    public void RefreshXPAmountRender() => xpAmountText.RefreshRender(localXpAmount.ToString());
    public void DistribuerXp(int xpAmount)
    {
        playerXp.GainXp(xpAmount);
        localXpAmount = playerXp.XpAmount;
        RefreshXPAmountRender();
    }
    public void UseXp(IStat statVis�, int nbUpgrade)
    {
        statVis�.UpgradeStat(nbUpgrade);
        playerXp.removeXp(CostForUpgrade * nbUpgrade);
    }
    //LocalChange to verify integrity
    public bool AddNbChanges(IStat stat)
    {
        if (localXpAmount - CostForUpgrade >= 0)
        {
            localXpAmount -= CostForUpgrade;
            DictioChangesStat[stat] += 1;
            RefreshXPAmountRender();
            return true;
        }
        return false;
    }
    public bool substractNbChanges(IStat stat)
    {
        if (localXpAmount + CostForUpgrade <= playerXp.XpAmount && DictioChangesStat[stat] > 0)
        {
            localXpAmount += CostForUpgrade;
            DictioChangesStat[stat] -= 1;
            RefreshXPAmountRender();
            return true;
        }
        return false;
    }
    //Aplies changes to stat
    public void ValidateChanges()
    {
        foreach (IStat stat in upgratableStats)
        {
            UseXp(stat, DictioChangesStat[stat]);
            if (typeof(IRechargeable).IsAssignableFrom(stat.GetType()))
            {
               var i = (IRechargeable)stat;
               i.Recharge();
            }
        }
        Reset();
    }
    public void Reset()
    {
        foreach (IStat stat in upgratableStats)
            DictioChangesStat[stat] = 0;

        localXpAmount = playerXp.XpAmount;
        RefreshXPAmountRender();
    }
}
