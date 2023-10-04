using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour
{
    Transform[] projectileLaunchers;
    [SerializeField] int nbLaunchers;
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
        Debug.Log(c.gameObject.name + "activated the trap");
        foreach(Transform t in projectileLaunchers) { Debug.Log(t.position.ToString()); }
    }

    
}
