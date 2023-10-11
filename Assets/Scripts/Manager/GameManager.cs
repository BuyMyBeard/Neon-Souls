using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BonfireManager bonfireManager;
    List<IRechargeable> rechargeables = new();

    private void Awake()
    {
        bonfireManager = GetComponent<BonfireManager>();
        rechargeables = FindObjectsOfType<MonoBehaviour>().OfType<IRechargeable>().ToList();
    }
    public void PlayerDie()
    {
        bonfireManager.Respawn();
        RechargeEverything();
    }
    public void RechargeEverything()
    {
        // TODO: Enemy Spawns , Piege Recharge
        foreach (IRechargeable rechargeable in rechargeables)
        {
            rechargeable.Recharge();
        }
    }
}
