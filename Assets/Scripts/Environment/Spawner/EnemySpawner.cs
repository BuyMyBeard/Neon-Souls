using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    [SerializeField] EnemyHealthbar healthbar;
    public override void Recharge()
    {
        GameObject enemy = Instantiate(entity);
        enemy.transform.SetPositionAndRotation(transform.position, transform.rotation);
    }
}
