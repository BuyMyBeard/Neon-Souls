using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IXpReceiver
{
    GameObject gameObject { get; }
    public int XpAmount { get; }
    public void GainXp(int amount);
    public void RemoveXp(int amount);
}
