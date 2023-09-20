using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    RespawnManager respawnManager;
    ButtonPrompt interactionManager;

    List<IRechargeable> rechargeables = new();

    private void Awake()
    {
        respawnManager = GetComponent<RespawnManager>();
        interactionManager = GetComponent<ButtonPrompt>();
        rechargeables = FindObjectsOfType<MonoBehaviour>().OfType<IRechargeable>().ToList();
    }
    public void PlayerDie()
    {
        respawnManager.Respawn();
        RechargeEverything();
    }
    public void RechargeEverything()
    {
        foreach (IRechargeable rechargeable in rechargeables)
        {
            rechargeable.Recharge();
        }
    }
}
