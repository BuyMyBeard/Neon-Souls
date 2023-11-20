using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHealth : EnemyHealth
{
    [SerializeField] float healthbarLingerTime = 2;
    public override void HandleHealthbar(int damage = 0)
    {
        displayHealthbar.Remove(damage, maxHealth, true);
    }
    protected override void Die()
    {
        invincible = true;
        animator.ResetAllTriggers();
        animator.SetTrigger("Die");
        animationEvents.DisableActions();
        animationEvents.FreezeMovement();
        animationEvents.FreezeRotation();
        animationEvents.StartIFrame();
        if (canFallApart) fallApart.DetachWeapons();
        StartCoroutine(HideBar());
    }

    IEnumerator HideBar()
    {
        yield return new WaitForSeconds(healthbarLingerTime);
        displayHealthbar.Hide();
    }
}
