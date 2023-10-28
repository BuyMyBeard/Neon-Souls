using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    BonfireManager bonfireManager;
    List<IRechargeable> rechargeables = new();
    readonly List<IRechargeable> tempRechargeables = new();
    [SerializeField] float timeBeforeRespawn = 5;
    [SerializeField] float timeBeforeDeathScreen = 2;
    DeathScreen deathScreen;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        Instance = this;
        bonfireManager = GetComponent<BonfireManager>();
        rechargeables = FindObjectsOfType<MonoBehaviour>().OfType<IRechargeable>().ToList();
        deathScreen = FindObjectOfType<DeathScreen>();
    }
    public void PlayerDie()
    {
        StartCoroutine(WaitBeforeResetting());
    }

    IEnumerator WaitBeforeResetting()
    {
        yield return new WaitForSeconds(timeBeforeDeathScreen);
        deathScreen.StartFadeIn();
        yield return new WaitForSeconds(timeBeforeRespawn - timeBeforeDeathScreen);
        deathScreen.SetVisible(false);
        bonfireManager.Respawn();
        RechargeEverything();
    }
    [ContextMenu("Recharge Everything")]
    public void RechargeEverything()
    {
        // TODO: Enemy Spawns , Piege Recharge
        foreach (IRechargeable rechargeable in rechargeables)
            rechargeable.Recharge();

        int count = tempRechargeables.Count;
        foreach(var rechargeable in tempRechargeables)
            rechargeable.Recharge();
    }
    /// <summary>
    /// Adds Object to list of objects that will be recharged at bonfire or death. These items should be destroyed after, since the list will be cleared.
    /// </summary>
    /// <param name="rechargeable">Rechargeable GameObject</param>
    public void AddTemporaryRechargeable(IRechargeable rechargeable)
    {
        tempRechargeables.Add(rechargeable);
    }

    public void RemoveTemporaryRechargeable(IRechargeable rechargeable)
    {
        tempRechargeables.Remove(rechargeable);
    }
}
