using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuXp : MonoBehaviour
{
    Dictionary<Type, float> DictioChangesStat = new();
    XpManager xpManager;

    private void Awake()
    {
        xpManager = FindObjectOfType<XpManager>();
        DictioChangesStat[typeof(Health)] = 2;
    }
    public void AddChanges(Type type,float ameliorateur)
    {
        DictioChangesStat[type] += ameliorateur;
    }
    public void ValidateChanges()
    {
        foreach ((Type type,float value) in DictioChangesStat)
        {
            xpManager.UseXp(type, value);
        }
    }
}
