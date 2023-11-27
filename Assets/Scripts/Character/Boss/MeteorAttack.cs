using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.TextCore.Text;

public class MeteorAttack : MonoBehaviour, IRechargeable
{
    [SerializeField] float hoverTime = 3;
    [SerializeField] float minCooldown = 30;
    [SerializeField] float maxCooldown = 45;
    [SerializeField] GameObject cracksDecal;
    [SerializeField] GameObject shadowDecal;
    [SerializeField] AnimationCurve screenShakeFreq;
    [SerializeField] AnimationCurve screenShakeAmp;
    [SerializeField] float screenShakeDuration = 1;
    Enemy enemy;
    EnemyAnimationEvents enemyAnimationEvents;
    EnemyMeleeAttack meleeAttack;
    Animator animator;
    NavMeshAgent agent;
    GameObject model;
    Health health;
    new Collider collider;
    GameObject shadow;
    public bool SkipThisFrameRootMotion { get; private set; } = false;

    private void Awake()
    {
        enemy = GetComponent<Enemy>();
        enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        animator = GetComponent<Animator>();
        agent = GetComponent<NavMeshAgent>();
        model = enemy.transform.GetChild(0).gameObject;
        collider = GetComponent<Collider>();
        health = GetComponent<Health>();
        meleeAttack = GetComponent<EnemyMeleeAttack>();
    }
    void Start()
    {
        StartCoroutine(nameof(AttackPeriodically));
    }

    [ContextMenu("Start Attack")]
    public void StartAttack()
    {
        if (health.IsDead) return;
        meleeAttack.actionQueued = true;
        animator.SetTrigger("MeteorLaunch");
        enemy.ChangeMode(Enemy.ModeId.Idle);
        enemyAnimationEvents.FreezeMovement();
        enemyAnimationEvents.FreezeRotation();
        enemyAnimationEvents.DisableActions();
        agent.enabled = false;
    }
    void ReachSky() => StartCoroutine(WaitInSky());
    void TakeOff() => collider.enabled = false;
    void SyncLocation() => StartCoroutine(DoSyncLocation());

    IEnumerator DoSyncLocation()
    {
        SkipThisFrameRootMotion = true;
        transform.position = new Vector3(enemy.Target.position.x, transform.position.y, enemy.Target.position.z) + Vector3.forward * 0.001f + animator.deltaPosition;
        transform.rotation = Quaternion.LookRotation(enemy.DirectionToPlayer, Vector3.up);
        shadow.transform.parent = null;
        shadow.transform.position = new Vector3(transform.position.x, shadow.transform.position.y, transform.position.z);
        yield return null;
        SkipThisFrameRootMotion = false;
    }

    IEnumerator WaitInSky()
    {
        model.SetActive(false);
        shadow = Instantiate(shadowDecal);
        Transform parent = enemy.Target.GetComponentInParent<CharacterController>().transform;
        shadow.transform.position = parent.position;
        shadow.transform.parent = parent;
        yield return new WaitForSeconds(hoverTime);

        animator.SetTrigger("MeteorAttack");
        model.SetActive(true);
    }

    void Land()
    {
        agent.enabled = true;
        collider.enabled = true;
        enemy.ChangeMode(Enemy.ModeId.InRange);
        ScreenShake.StartShake(screenShakeAmp, screenShakeFreq, screenShakeDuration);
        Instantiate(cracksDecal).transform.position = transform.position;
    }
    IEnumerator AttackPeriodically()
    {
        yield return new WaitUntil(() => health.CurrentHealth / health.MaxHealth < .5f);
        animator.SetBool("ExtendAttacks", true);
        while (!health.IsDead)
        {
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
            StartAttack();
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
            meleeAttack.actionQueued = false;
            yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        }
    }

    public void Recharge(RechargeType rechargeType)
    {
        meleeAttack.actionQueued = false;
        StopCoroutine(nameof(AttackPeriodically));
        StartCoroutine(nameof(AttackPeriodically));
    }
}
