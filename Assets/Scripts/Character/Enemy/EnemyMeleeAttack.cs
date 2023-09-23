using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMeleeAttack : MeleeAttack
{
    float timeElapsed = 0;
    protected override void DamageOpponent(Health opponentHealth)
    {
        opponentHealth.InflictDamage(15);
    }

    private void Update()
    {
        timeElapsed += Time.deltaTime;
        if (timeElapsed > 4)
        {
            timeElapsed = 0;
            animator.SetTrigger("LightAttack");
        }
    }
}
