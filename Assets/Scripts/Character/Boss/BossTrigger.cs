using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BossTrigger : MonoBehaviour, IRechargeable
{
    BossManager bossManager;
    Collider bossTrigger;
    PlayerAnimationEvents playerAnimationEvents;
    [SerializeField] float timeBeforeCutsceneStart;

    private void Awake()
    {
        bossManager = FindAnyObjectByType<BossManager>();
        bossTrigger = GetComponent<Collider>();
        playerAnimationEvents = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerAnimationEvents>();
    }
    private void OnTriggerEnter(Collider other)
    {
        playerAnimationEvents.FreezeMovement();
        playerAnimationEvents.FreezeRotation();
        Invoke("StartCutScene", timeBeforeCutsceneStart);
        bossTrigger.enabled = false;
    }
    private void StartCutScene() => bossManager.StartCutscene();
    public void Recharge(RechargeType rechargeType = RechargeType.Respawn)
    {
        bossTrigger.enabled = true;
    }
}
