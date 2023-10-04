using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivation : MonoBehaviour
{
    [SerializeField] bool wasTriggered = false;
    void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer != 3) // 3 == Environment Layer
        {
            gameObject.GetComponentInParent<TrapProjectile>().PullTrigger(c);
            gameObject.GetComponent<Collider>().enabled = false;
            gameObject.transform.position = new Vector3(transform.position.x, -.25f, transform.position.z);
        }   
    }
    public void ResetTrap()
    {
        gameObject.GetComponent <Collider>().enabled = true;
        gameObject.transform.position = new Vector3(transform.position.x, 0, transform.position.z);
    }
}
