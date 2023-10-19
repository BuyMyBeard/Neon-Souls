using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ValidateStatChange : MonoBehaviour
{
    XpManager xpManager;
    private void Awake()
    {
        xpManager = FindObjectOfType<XpManager>();
    }
    public void ValidateUsesOfXp()
    {
        Debug.Log("Validate XpUsage");
    }
}
