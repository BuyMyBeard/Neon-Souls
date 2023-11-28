using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour,Trap
{
    List<Transform> projectileLaunchers = new ();
    [SerializeField] GameObject bullet;
    [SerializeField] float bulletSpawnRate = 1;
    [SerializeField] int bulletSalvo = 3;
    [SerializeField] float bullerLifeSpan = 1;
    private void Awake()
    {
        foreach(Transform child in transform)
        {
            projectileLaunchers.Add(child);
        }
    }
    public void Trigger()
    {
        StartCoroutine(ProjectilesCoroutine());
    }
    IEnumerator ProjectilesCoroutine()
    {
        for (int i = 0; i < bulletSalvo; i++)
        {
            LaunchProjectiles();
            yield return new WaitForSeconds(bulletSpawnRate);
        }
    }
    void LaunchProjectiles()
    {
        foreach(Transform t in projectileLaunchers) 
        {
            GameObject liveBullet = Instantiate(bullet, t.position, t.rotation);
            liveBullet.transform.LookAt(t.position + t.forward.normalized);
            TrapBullet i = liveBullet.GetComponent<TrapBullet>();
           
            if (i == null)
                throw new MissingComponentException();
            i.lifeSpan = bullerLifeSpan;
            i.MoveBullet(t.forward);
        }
    }
}
