using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossManager : MonoBehaviour, IRechargeable
{
    [SerializeField] BossHealth boss1Health;
    [SerializeField] BossHealth boss2Health;

    [SerializeField] DisplayBar bossBar1;
    [SerializeField] DisplayBar bossBar2;

    Animator animator1;
    Animator animator2;
    EnemyAnimationEvents boss1Events;
    EnemyAnimationEvents boss2Events;
    Enemy boss1;
    Enemy boss2;
    PlayerAnimationEvents playerAnimationEvents;
    bool defeated = false;
    bool cinematicInProgress = false;
    void Awake()
    {
        playerAnimationEvents = FindObjectOfType<PlayerAnimationEvents>();
        boss1 = boss1Health.GetComponent<Enemy>();
        boss2 = boss2Health.GetComponent<Enemy>();
        animator1 = boss1.GetComponent<Animator>();
        animator2 = boss2.GetComponent<Animator>();
        boss1Events = boss1.GetComponent<EnemyAnimationEvents>();
        boss2Events = boss2.GetComponent<EnemyAnimationEvents>();
    }
    void Start()
    {
        InitBosses();
    }
    [ContextMenu("Start Boss Fight")]
    public void StartBossFight()
    {
        if (defeated) return;
        StartCoroutine(nameof(BossFight));
    }

    IEnumerator BossFight()
    {
        cinematicInProgress = true;
        //playerAnimationEvents.DisableActions();
        //playerAnimationEvents.FreezeCamera();
        // yield return new WaitWhile(() => cinematicInProgress);
        yield return new WaitForSeconds(2);
        animator1.speed = 1;
        animator2.speed = 1;
        yield return new WaitForSeconds(4);
        // playerAnimationEvents.ResetAll();
        animator1.SetTrigger("StopPosing");
        animator2.SetTrigger("StopPosing");
        yield return new WaitForSeconds(.5f);
        boss1.ChangeMode(Enemy.ModeId.InRange);
        boss2.ChangeMode(Enemy.ModeId.InRange);
        boss1Events.EnableActions();
        boss2Events.EnableActions();
        boss1Events.UnFreezeMovement();
        boss2Events.UnFreezeMovement();
        yield return new WaitWhile(() => boss1Health.CurrentHealth > 0 && boss2Health.CurrentHealth > 0);
        defeated = true;
    }
    public void Recharge()
    {
        if (defeated) return;
        StopCoroutine(nameof(BossFight));
        InitBosses();
    }

    void InitBosses()
    {
        boss1Events.DisableActions();
        boss2Events.DisableActions();
        boss1Events.FreezeMovement();
        boss2Events.FreezeMovement();
        animator1.Play("PoseLeft");
        animator1.speed = 0;
        animator2.Play("PoseRight");
        animator2.speed = 0;
    }
    public void EndCinematic() => cinematicInProgress = false;
}
