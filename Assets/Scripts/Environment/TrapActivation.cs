using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivation : MonoBehaviour
{
    //TO DO : doit recharger avec la mort ou un siting 
    public bool wasTriggered = false;
    void OnTriggerEnter(Collider c)
    {
        if(wasTriggered) return;

        gameObject.GetComponent<Trap>().Trigger();
        gameObject.GetComponent<Collider>().enabled = false;
        wasTriggered = true;
    }
    public void ResetTrap()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        wasTriggered = false;
    }
}