using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IStat
{ 
    GameObject gameObject { get; }
    public float Value {get;}
    int Upgrade { get;}
    public void UpgradeStat(int nbAmelioration);
}
