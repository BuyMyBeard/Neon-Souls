using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public interface IXpGiver
{
    public void GiveXp();
}
public interface IXpReceiver
{
    GameObject gameObject { get;}
    public void GainXp(int xp);
}
public class XpManager : MonoBehaviour
{
    List<IXpReceiver> xpReceivers = new();
    [SerializeField] List<IStat> upgratableStats = new();
    private void Start()
    {
        xpReceivers = FindObjectsOfType<MonoBehaviour>().OfType<IXpReceiver>().ToList();
        xpReceivers.ForEach(delegate(IXpReceiver xpReceiver) {
            upgratableStats.AddRange(xpReceiver.gameObject.GetComponents<IStat>());
        });
    }
    public void DistributeXp(int xpAmount)
    {
        foreach (var receiver in xpReceivers)
        {
            receiver.GainXp(xpAmount);
        }
    }

    public void UseXp(float upgradeValue)
    {

    }
}
