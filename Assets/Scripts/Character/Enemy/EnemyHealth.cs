using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class EnemyHealth : Health
{
    public Transform healthbarContainer;
    public Coroutine showHealthbarCoroutine = null;
    public int healthBarDisplayCounter = 0;
    bool staggerable;
    Stagger stagger;

    protected override void Awake()
    {
        base.Awake();
        staggerable = TryGetComponent(out stagger);
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
    IEnumerator ShowHealthbarTemporarily(float time)
    {
        ShowHealthbar();
        yield return new WaitForSeconds(time);
        showHealthbarCoroutine = null;
        HideHealthbar();
    }
    public override void HandleHealthbar(int damage)
    {
        if (showHealthbarCoroutine != null)
        {
            StopCoroutine(showHealthbarCoroutine);
            healthBarDisplayCounter--;
        }
        showHealthbarCoroutine = StartCoroutine(ShowHealthbarTemporarily(timeShowingHealthbar));
        displayHealthbar.Remove(damage, maxHealth, true);
    }
    public void ShowHealthbar()
    {
        Debug.Log("Counter increased");
        healthBarDisplayCounter++;
        if (displayHealthbar != null)
        {
            return;
        }
        displayHealthbar = GameManager.enemyHealthbarsPool.SpawnObject(null, out _).GetComponent<EnemyHealthbar>();
        (displayHealthbar as EnemyHealthbar).TrackedEnemy = healthbarContainer;
        displayHealthbar.Show();
    }
    public void HideHealthbar()
    {
        Debug.Log("Counter decreased");
        healthBarDisplayCounter--;
        if (healthBarDisplayCounter <= 0)
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
}
