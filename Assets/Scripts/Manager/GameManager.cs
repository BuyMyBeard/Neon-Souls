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
    float timeBeforeRespawn = 5;

    private void Awake()
    {
        bonfireManager = GetComponent<BonfireManager>();
        rechargeables = FindObjectsOfType<MonoBehaviour>().OfType<IRechargeable>().ToList();
    }
    public void PlayerDie()
    {
        StartCoroutine(WaitBeforeResetting());
    }

    IEnumerator WaitBeforeResetting()
    {
        yield return new WaitForSeconds(timeBeforeRespawn);
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
