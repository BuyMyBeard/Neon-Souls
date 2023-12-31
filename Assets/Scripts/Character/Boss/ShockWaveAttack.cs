using System.Collections;
using Unity.VisualScripting;
using UnityEngine;

public class ShockWaveAttack : MonoBehaviour, IRechargeable
{
    [SerializeField] Material shockWaveMaterial;
    [SerializeField] int baseDamage = 80;
    [SerializeField] float damageInflictedNormalizedTime = 0f;
    [SerializeField] float attackRadius = 10f;
    [SerializeField] float duration = 1f;
    [SerializeField] float initialStrength = -.7f;
    [SerializeField] float finalStrength = 0f;
    [SerializeField] float initialDistance = -0.1f;
    [SerializeField] float finalDistance = 2f;
    [SerializeField] float size = .05f;
    [SerializeField] float minCooldown = 30;
    [SerializeField] float maxCooldown = 45;
    [SerializeField] GameObject smallCracks;
    [SerializeField] GameObject largeCracks;
    [SerializeField] AnimationCurve screenShakeFreq;
    [SerializeField] AnimationCurve screenShakeAmp;
    [SerializeField] float screenShakeDuration = 2;
    PlayerHealth playerHealth;
    Enemy enemy;
    Health health;
    EnemyAnimationEvents enemyAnimationEvents;
    EnemyMeleeAttack meleeAttack;
    Animator animator;
    bool firstWave = false;
    private void Awake()
    {
        playerHealth = FindObjectOfType<PlayerHealth>();
        enemy = GetComponent<Enemy>();
        enemyAnimationEvents = GetComponent<EnemyAnimationEvents>();
        animator = GetComponent<Animator>();
        health = GetComponent<Health>();
        meleeAttack = GetComponent<EnemyMeleeAttack>();
    }
    public void StartAttackPeriodically()
    {
        StopCoroutine(nameof(AttackPeriodically));
        StartCoroutine(nameof(AttackPeriodically));
    }
    IEnumerator Shockwave()
    {
        bool hasDoneDamage = false;
        Instantiate(firstWave? smallCracks : largeCracks, transform.position, smallCracks.transform.rotation);

        shockWaveMaterial.SetFloat("_Size", size);
        ScreenShake.StartShake(screenShakeAmp, screenShakeFreq, screenShakeDuration);
        Haptics.ExplosionLong();
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            if (Time.timeScale == 0)
            {
                shockWaveMaterial.SetFloat("_ShockWaveStrength", 0);
                shockWaveMaterial.SetFloat("_WaveDistanceFromCenter", -.1f);
            }
            else
            {
                shockWaveMaterial.SetVector("_RingSpawnPosition", Camera.main.WorldToViewportPoint(transform.position));
                shockWaveMaterial.SetFloat("_ShockWaveStrength", Mathf.Lerp(initialStrength, finalStrength, t));
                shockWaveMaterial.SetFloat("_WaveDistanceFromCenter", Mathf.Lerp(initialDistance, finalDistance, t));
                if (!hasDoneDamage && t >= damageInflictedNormalizedTime)
                {
                    hasDoneDamage = true;
                    InflictDamage();
                }
            }
            yield return null;
        }
        ResetShockWaveMaterial();
        firstWave = false;
    }

    public void StartShockwave() => StartCoroutine(Shockwave());

    [ContextMenu("Start Attack")]
    public void StartAttack()
    {
        if (health.IsDead) return;
        firstWave = true;
        animator.SetTrigger("ShockwaveAttack");
        enemyAnimationEvents.FreezeMovement();
        enemyAnimationEvents.FreezeRotation();
        enemyAnimationEvents.DisableActions();
        meleeAttack.actionQueued = false;
    }
    private void OnDestroy()
    {
        ResetShockWaveMaterial();
    }
    private void ResetShockWaveMaterial()
    {
        shockWaveMaterial.SetFloat("_ShockWaveStrength", 0);
        shockWaveMaterial.SetFloat("_WaveDistanceFromCenter", -.1f);
    }
    private void InflictDamage()
    {
        if (enemy.DistanceFromPlayer < attackRadius)
            playerHealth.InflictDamage(baseDamage, transform);
    }

    IEnumerator AttackPeriodically()
    {
        yield return new WaitUntil(() => health.CurrentHealth / health.MaxHealth < .5f);
        meleeAttack.actionQueued = true;
        while (!health.IsDead)
        {
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
            StartAttack();
            animator.SetBool("ExtendAttacks", true);
            yield return new WaitUntil(() => enemyAnimationEvents.ActionAvailable);
            meleeAttack.actionQueued = false;
            yield return new WaitForSeconds(Random.Range(minCooldown, maxCooldown));
        }
    }

    public void Recharge(RechargeType rechargeType)
    {
        meleeAttack.actionQueued = false;
    }
}
