using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivation : MonoBehaviour
{
    [SerializeField] bool wasTriggered = false;
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer != 3)
        {
            gameObject.GetComponentInParent<TrapProjectile>().PullTrigger(c);
            gameObject.GetComponent<Collider>().enabled = false;
        }
        
    }
}
