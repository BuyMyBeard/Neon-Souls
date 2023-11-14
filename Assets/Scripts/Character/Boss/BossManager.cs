using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour, IRechargeable
{
    [SerializeField] BossHealth boss1;
    [SerializeField] BossHealth boss2;

    [SerializeField] DisplayBar bossBar1;
    [SerializeField] DisplayBar bossBar2;

    PlayerAnimationEvents playerAnimationEvents;
    bool defeated = false;
    bool cinematicInProgress = false;
    void Awake()
    {
        playerAnimationEvents = FindObjectOfType<PlayerAnimationEvents>();
    }
    public void StartBossFight()
    {
        if (defeated) return;
        StartCoroutine(nameof(BossFight));
    }

    IEnumerator BossFight()
    {
        cinematicInProgress = true;
        playerAnimationEvents.DisableActions();
        playerAnimationEvents.FreezeCamera();
        yield return new WaitWhile(() => cinematicInProgress);
        playerAnimationEvents.ResetAll();
        yield return new WaitWhile(() => boss1.CurrentHealth > 0 && boss2.CurrentHealth > 0);
        defeated = true;
    }
    public void Recharge()
    {
        if (defeated) return;
        StopCoroutine(nameof(BossFight));
    }
    public void EndCinematic() => cinematicInProgress = false;
}
