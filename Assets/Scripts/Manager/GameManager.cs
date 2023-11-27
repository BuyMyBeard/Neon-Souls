using System.Collections;
using System.Collections.Generic;
using System.Linq;
//using UnityEditor.PackageManager;
using UnityEngine;
public enum RechargeType { Respawn, Rest }

public class GameManager : MonoBehaviour
{
    static public ObjectPool enemyHealthbarsPool;
    BonfireManager bonfireManager;
    List<IRechargeable> rechargeables = new();
    readonly List<IRechargeable> tempRechargeables = new();
    [SerializeField] float timeBeforeRespawn = 5;
    [SerializeField] float timeBeforeDeathScreen = 2;
    DeathScreen deathScreen;
    public static GameManager Instance { get; private set; }

    private void Awake()
    {
        enemyHealthbarsPool = GameObject.FindGameObjectWithTag("EnemyHealthbarPool").GetComponent<ObjectPool>();
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
    public void RechargeEverything(RechargeType rechargeType = RechargeType.Respawn)
    {
        foreach (IRechargeable rechargeable in rechargeables)
            rechargeable.Recharge(rechargeType);

        foreach(var rechargeable in tempRechargeables)
            rechargeable.Recharge(rechargeType);
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
