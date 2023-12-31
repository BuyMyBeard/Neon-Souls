using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;
using UnityEngine.SceneManagement;

[RequireComponent(typeof(AudioSource))]
public class BossManager : MonoBehaviour, IRechargeable
{
    [SerializeField] BossHealth boss1Health;
    [SerializeField] BossHealth boss2Health;
    [SerializeField] float fadeSpeed;
    [SerializeField] float volume;
    DisplayBar bossbar1;
    DisplayBar bossbar2;

    Animator animator1;
    Animator animator2;
    Animator cutscene;
    EnemyAnimationEvents boss1Events;
    EnemyAnimationEvents boss2Events;
    Enemy boss1;
    Enemy boss2;
    PlayerAnimationEvents playerAnimationEvents;
    FadeFilter fadeFilter;
    AudioSource audioSource;
    ZoneTransitionManager zoneTransitionManager;
    public bool CutsceneInProgress { get; private set; } = false;
    bool defeated = false;
    void Awake()
    {
        fadeFilter = GetComponentInChildren<FadeFilter>();
        playerAnimationEvents = FindObjectOfType<PlayerAnimationEvents>();
        boss1 = boss1Health.GetComponent<Enemy>();
        boss2 = boss2Health.GetComponent<Enemy>();
        animator1 = boss1.GetComponent<Animator>();
        animator2 = boss2.GetComponent<Animator>();
        cutscene = GetComponent<Animator>();
        boss1Events = boss1.GetComponent<EnemyAnimationEvents>();
        boss2Events = boss2.GetComponent<EnemyAnimationEvents>();
        bossbar1 = GameObject.FindGameObjectWithTag("BossBar1").GetComponent<DisplayBar>();
        bossbar2 = GameObject.FindGameObjectWithTag("BossBar2").GetComponent<DisplayBar>();
        boss1Health.displayHealthbar = bossbar1;
        boss2Health.displayHealthbar = bossbar2;
        audioSource = GetComponent<AudioSource>();
        zoneTransitionManager = FindObjectOfType<ZoneTransitionManager>();
    }
    void Start()
    {
        StartCoroutine(ResetBosses());
    }
        
    [ContextMenu("Start Boss Fight")]
    public void StartCutscene()
    {
        if (defeated) return;
        cutscene.Play("Cutscene");
        CutsceneInProgress = true;
        zoneTransitionManager.FadeNonBossesOut();
        zoneTransitionManager.FadeCurrentZone();
    }
    void OnFadedToBlack()
    {
        playerAnimationEvents.FreezeCamera();
        playerAnimationEvents.FreezeMovement();
        playerAnimationEvents.DisableActions();
        playerAnimationEvents.FreezeRotation();
    }
    void OnFadeFromBlack()
    {
        audioSource.volume = volume;
        audioSource.Play();
    }
    void StartBossAnimation()
    {
        animator1.speed = 1;
        animator2.speed = 1;
    }
    void StopPosing()
    {
        animator1.SetTrigger("StopPosing");
        animator2.SetTrigger("StopPosing");
    }
    void ActivateBosses()
    {
        boss1.ChangeMode(Enemy.ModeId.InRange);
        boss1.GetComponent<MeteorAttack>().StartAttackPeriodically();
        boss2.ChangeMode(Enemy.ModeId.InRange);
        boss2.GetComponent<ShockWaveAttack>().StartAttackPeriodically();
        boss1Events.EnableActions();
        boss2Events.EnableActions();
        boss1Events.UnFreezeMovement();
        boss2Events.UnFreezeMovement();
    }
    public void Recharge(RechargeType rechargeType)
    {
        // Doesn't work for now because conflict with other Recharge method for enemy
        if (defeated)
            return;

        StartCoroutine(ResetBosses());
    }

    IEnumerator ResetBosses()
    {
        StartCoroutine(AudioFadeUtils.FadeOut(audioSource, fadeSpeed, true));
        bossbar1.Hide();
        bossbar2.Hide();
        bossbar1.Set(1, 1);
        bossbar2.Set(1, 1);
        yield return null;
        boss1Events.DisableActions();
        boss2Events.DisableActions();
        boss1Events.FreezeMovement();
        boss2Events.FreezeMovement();
        animator1.ResetAllTriggers();
        animator2.ResetAllTriggers();
        animator1.ResetAllBooleans();
        animator2.ResetAllBooleans();
        animator1.SetBool("ExtendAttacks", false);
        animator2.SetBool("ExtendAttacks", false);
        StopCoroutine(nameof(WaitForDeath));
        boss1Health.invincible = true;
        boss2Health.invincible = true;
        animator1.Play("PoseLeft");
        animator1.speed = 0;
        animator2.Play("PoseRight");
        animator2.speed = 0;
    }
    void StopCutscene()
    {
        Time.timeScale = 1;
        playerAnimationEvents.ResetAll();
        ActivateBosses();
        CutsceneInProgress = false;
        bossbar1.Show();
        bossbar2.Show();
        boss1Health.invincible = false;
        boss2Health.invincible = false;
        StartCoroutine(nameof(WaitForDeath));
    }

    IEnumerator WaitForDeath()
    {
        yield return new WaitWhile(() => boss1Health.CurrentHealth > 0 || boss2Health.CurrentHealth > 0);
        defeated = true;
        yield return new WaitForSeconds(3);
        VolumeProfile profile = Camera.main.GetComponent<Volume>().profile;
        profile.TryGet(out Bloom bloom);
        while (bloom.intensity.value < 5000)
        {
            bloom.intensity.value += 1500 * Time.deltaTime;
            yield return null;
        }
        fadeFilter.StartFadeIn(3);
        zoneTransitionManager.FadeEverythingOut();
        yield return new WaitForSeconds(4);
        SceneManager.LoadScene(2);
    }
}
