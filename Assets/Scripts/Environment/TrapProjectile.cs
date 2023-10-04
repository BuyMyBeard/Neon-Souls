using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour
{
    Transform[] projectileLaunchers;
    [SerializeField] int nbLaunchers;
    [SerializeField] GameObject bullet;
    private void Awake()
    {
        projectileLaunchers = new Transform[nbLaunchers];
        for(int i = 0; i < nbLaunchers; i++)
        {
            projectileLaunchers[i] = transform.GetChild(i);
        }
        
    }
    public void PullTrigger(Collider c)
    {
        foreach(Transform t in projectileLaunchers) 
        {
            GameObject liveBullet = Instantiate(bullet, t.position, t.rotation);
            liveBullet.GetComponent<TrapBullet>().MoveBullet(t.forward);
            Debug.Log(liveBullet.transform.position.ToString());
        }
    }

    
}
