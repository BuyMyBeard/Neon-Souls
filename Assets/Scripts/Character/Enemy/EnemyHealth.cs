using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHealth : Health
{
    public Transform healthbarContainer;
    public Coroutine showHealthbarCoroutine = null;
    public int healthBarDisplayCounter = 0;
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
        healthBarDisplayCounter--;
        if (healthBarDisplayCounter <= 0)
        {
            displayHealthbar.Hide();
            GameManager.enemyHealthbarsPool.ReturnObject(displayHealthbar.gameObject);
            displayHealthbar = null;
        }
    }
}
