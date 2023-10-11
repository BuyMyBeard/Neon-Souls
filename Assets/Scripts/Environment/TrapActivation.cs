using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapActivation : MonoBehaviour
{
    //TO DO : doit recharger avec la mort ou un siting 
    public bool wasTriggered = false;
    Transform physicalSwitch;
    Vector3 physicalSwitchInitalPosition;
    private void Awake()
    {
        physicalSwitch = transform.parent.GetChild(1);
        physicalSwitchInitalPosition = physicalSwitch.position;
    }
    void OnTriggerEnter(Collider c)
    {
        if(wasTriggered) return;

        gameObject.GetComponent<Trap>().Trigger();
        gameObject.GetComponent<Collider>().enabled = false;
        StartCoroutine(LerpPosition());
        wasTriggered = true;
    }
    public void ResetTrap()
    {
        gameObject.GetComponent<Collider>().enabled = true;
        physicalSwitch.position = physicalSwitchInitalPosition;
        wasTriggered = false;
    }
    IEnumerator LerpPosition()
    {
        float timeElapsed = 0;
        float lerpDuration = 3;
        Vector3 endPosition = new Vector3(physicalSwitchInitalPosition.x, physicalSwitchInitalPosition.y - 0.5f, physicalSwitchInitalPosition.z);
        while(timeElapsed < lerpDuration)
        {
            physicalSwitch.position = Vector3.Lerp(physicalSwitchInitalPosition, endPosition, timeElapsed / lerpDuration);
            timeElapsed += Time.deltaTime;
            yield return null;
        }
        physicalSwitch.position = endPosition;
    }
}