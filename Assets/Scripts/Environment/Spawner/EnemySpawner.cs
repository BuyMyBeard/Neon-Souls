using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemySpawner : Spawner
{
    public override void Recharge()
    {
        GameObject enemy = Instantiate(entity);
        enemy.transform.position = transform.position;
        enemy.transform.rotation = transform.rotation;
    }
}
