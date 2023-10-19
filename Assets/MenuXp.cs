using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuXp : MonoBehaviour
{
    Dictionary<Type, float> DictioChangesStat = new();

    private void Awake()
    {
        DictioChangesStat[typeof(Health)] = 2;
    }
    public void AddChanges(Type type,float ameliorateur)
    {

    }
}
