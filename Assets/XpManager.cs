using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

interface IXpGiver
{
    public void GiveXp();
}
interface IXpReceiver
{
    public void GainXp(int xp);
}
public class XpManager : MonoBehaviour
{
    List<IXpReceiver> xpReceivers = new();

    private void Start()
    {
        xpReceivers = FindObjectsOfType<MonoBehaviour>().OfType<IXpReceiver>().ToList();
    }
    public void DistributeXp(int xpAmount)
    {
        foreach (var receiver in xpReceivers)
        {
            receiver.GainXp(xpAmount);
        }
    }
}
