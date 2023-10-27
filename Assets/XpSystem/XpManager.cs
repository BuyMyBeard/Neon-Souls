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


    float localXpAmout;

    private void Start()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        upgratableStats.AddRange(playerXp.GetComponents<IStat>());
        xpAmountText = FindObjectOfType<XpAmountText>();
        
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
        xpAmountText.RefreshRender(localXpAmout.ToString()); 
    }
    //ChangePlayer and stat
    public void DistribuerXp(int xpAmount)
    {
        playerXp.GainXp(xpAmount);
        localXpAmout = playerXp.XpAmount;
        xpAmountText.RefreshRender(localXpAmout.ToString());
    }
    public void UseXp(IStat statVis�, int nbUpgrade)
    {
        statVis�.UpgradeStat(nbUpgrade);
        playerXp.removeXp(CostForUpgrade * nbUpgrade);

        localXpAmout = playerXp.XpAmount;
        xpAmountText.RefreshRender(localXpAmout.ToString());
    }
    //LocalChange to verify integrity
    public bool AddNbChanges(IStat stat)
    {
        if (localXpAmout - CostForUpgrade >= 0)
        {
            localXpAmout -= CostForUpgrade;
            DictioChangesStat[stat] += 1;
            xpAmountText.RefreshRender(localXpAmout.ToString());
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
            xpAmountText.RefreshRender(localXpAmout.ToString());
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
