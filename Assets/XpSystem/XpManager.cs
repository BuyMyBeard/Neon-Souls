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

    public void RefreshXPAmountRender() => xpAmountText.RefreshRender(localXpAmount.ToString());
    private IEnumerator Start()
    {
        foreach (IStat stat in upgratableStats)
        {
            DictioChangesStat.Add(stat, 0);
        }
        yield return null;
        localXpAmount = playerXp.XpAmount;
    }
    //ChangePlayer and stat
    public void DistribuerXp(int xpAmount)
    {
        playerXp.GainXp(xpAmount);
        localXpAmount = playerXp.XpAmount;
        xpAmountText.RefreshRender(localXpAmount.ToString());
    }
    public void UseXp(IStat statVisé, int nbUpgrade)
    {
        statVisé.UpgradeStat(nbUpgrade);
        playerXp.removeXp(CostForUpgrade * nbUpgrade);

        localXpAmount = playerXp.XpAmount;
        xpAmountText.RefreshRender(localXpAmount.ToString());
    }
    //LocalChange to verify integrity
    public bool AddNbChanges(IStat stat)
    {
        if (localXpAmount - CostForUpgrade >= 0)
        {
            localXpAmount -= CostForUpgrade;
            DictioChangesStat[stat] += 1;
            xpAmountText.RefreshRender(localXpAmount.ToString());
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
            xpAmountText.RefreshRender(localXpAmount.ToString());
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
            DictioChangesStat[stat] = 0;
            if (typeof(IRechargeable).IsAssignableFrom(stat.GetType()))
            {
               var i = (IRechargeable)stat;
               i.Recharge();
            }
        }
    }
}
