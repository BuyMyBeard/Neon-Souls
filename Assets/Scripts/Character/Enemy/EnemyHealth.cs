using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : Health
{
    public Enemy enemy;
    public Transform healthbarContainer;
    public Coroutine showHealthbarCoroutine = null;
    public int healthBarDisplayCounter = 0;
    public bool hasAlreadyDiedOnce { get; private set; } = false;

    protected override void Awake()
    {
        base.Awake();
        enemy = GetComponent<Enemy>();
    }
    private void OnDisable()
    {
        if (showHealthbarCoroutine != null)
        {
            StopCoroutine(showHealthbarCoroutine);
            showHealthbarCoroutine = null;
            HideHealthbar();
        }
    }
    IEnumerator ShowHealthbarTemporarily(float time, int damageInflicted = 0)
    {
        ShowHealthbar(damageInflicted);
        yield return new WaitForSeconds(time);
        showHealthbarCoroutine = null;
        HideHealthbar();
    }
    public override void HandleHealthbar(int damage = 0)
    {
        if (showHealthbarCoroutine != null)
        {
            StopCoroutine(showHealthbarCoroutine);
            healthBarDisplayCounter--;
        }
        showHealthbarCoroutine = StartCoroutine(ShowHealthbarTemporarily(timeShowingHealthbar, damage));
        displayHealthbar.Remove(damage, maxHealth, true);
    }
    public void ShowHealthbar(int damage = 0)
    {
        healthBarDisplayCounter++;
        if (displayHealthbar != null)
        {
            return;
        }
        displayHealthbar = GameManager.enemyHealthbarsPool.SpawnObject(null, out _).GetComponent<EnemyHealthbar>();
        (displayHealthbar as EnemyHealthbar).TrackedEnemy = healthbarContainer;
        displayHealthbar.Show(damage);
    }
    public void HideHealthbar()
    {
        healthBarDisplayCounter--;
        if (healthBarDisplayCounter <= 0 && displayHealthbar != null)
        {
            displayHealthbar.Hide();
            GameManager.enemyHealthbarsPool.ReturnObject(displayHealthbar.gameObject);
            displayHealthbar = null;
        }
    }
    public override void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition, BlockSound blockSound = BlockSound.SwordClash)
    {
        InflictDamage(damage);
        if (staggerable)
            stagger.BecomeStaggered(attackerPosition, 1);
    }
    protected override void Die()
    {
        base.Die();
        hasAlreadyDiedOnce = true;
        GetComponent<Enemy>().GiveXp();
    }
}
