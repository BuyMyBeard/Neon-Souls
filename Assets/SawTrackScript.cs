using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawTrackScript : MonoBehaviour
{
    bool isActive = false;
    List<Transform> saws = new();
    // Start is called before the first frame update
    void Start()
    {
        foreach(Transform t in transform)
            saws.Add(t.GetChild(0));
        foreach(Transform t in saws)
            t.gameObject.SetActive(false);

    }
    private void OnTriggerEnter(Collider c)
    {
        if (c.gameObject.layer == 10) //Player layer
        {
            isActive = true;
            StartCoroutine(LaunchSaws());
        }

    }
    IEnumerator LaunchSaws()
    {
        foreach (Transform t in saws)
            t.gameObject.GetComponent<SawScript>().MoveSaw();// plante ici, jsuis fatigué.
        yield return null;
    }
}
