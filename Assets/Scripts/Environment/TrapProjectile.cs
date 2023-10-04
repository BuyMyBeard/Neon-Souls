using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapProjectile : MonoBehaviour
{
    Transform projectileLauncher0;
    Transform projectileLauncher1;
    Transform projectileLauncher2;
    private void Awake()
    {
        projectileLauncher0 = transform.GetChild(0);
        projectileLauncher1 = transform.GetChild(1);
        projectileLauncher2 = transform.GetChild(2);
    }
    public void PullTrigger(Collider c)
    {
        Debug.Log(c.gameObject.name + "activated the trap");
        Debug.Log(projectileLauncher0.position.ToString());
        Debug.Log(projectileLauncher1.position.ToString());
        Debug.Log(projectileLauncher2.position.ToString());
    }
    
}
