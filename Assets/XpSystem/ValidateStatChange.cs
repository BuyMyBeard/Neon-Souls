using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class ValidateStatChange : MonoBehaviour
{
    XpManager xpManager;
    XpMenuManager xpMenu;
    private void Awake()
    {
        xpManager = FindObjectOfType<XpManager>();
        xpMenu = FindObjectOfType<XpMenuManager>();
    }
    public void ValidateUsesOfXp()
    {
        xpManager.ValidateChanges();
        xpMenu.Hide();
        Debug.Log("Validate XpUsage");
    }
}
