using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] EnemyHealthbar healthbar;
    public override void Recharge()
    {
        GameObject enemy = Instantiate(entity);
        enemy.GetComponent<Health>().displayHealthbar = healthbar;
        healthbar.TrackedEnemy = enemy.transform;
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
    }
}
