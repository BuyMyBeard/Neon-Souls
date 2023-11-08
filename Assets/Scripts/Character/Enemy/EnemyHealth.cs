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
    bool staggerable;
    Stagger stagger;

    protected override void Awake()
    {
        base.Awake();
        staggerable = TryGetComponent(out stagger);
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
        Debug.Log("Counter increased");
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
        Debug.Log("Counter decreased");
        healthBarDisplayCounter--;
        if (healthBarDisplayCounter <= 0 && displayHealthbar != null)
        {
            displayHealthbar.Hide();
            GameManager.enemyHealthbarsPool.ReturnObject(displayHealthbar.gameObject);
            displayHealthbar = null;
        }
    }
    public override void InflictBlockableDamage(int damage, int staminaBlockCost, Transform attackerPosition)
    {
        InflictDamage(damage);
        if (staggerable)
            stagger.BecomeStaggered(attackerPosition, 1);
    }
    protected override void Die()
    {
        base.Die();
        GetComponent<Enemy>().GiveXp();
    }
}
