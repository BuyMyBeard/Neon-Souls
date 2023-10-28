using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour
{
    [ContextMenu("Decompose")]
    public void Decompose()
    {
        GameObject body = GetComponentInChildren<Animator>().gameObject;
        GameObject dummy = Instantiate(body);
        dummy.transform.position = body.transform.position;
        dummy.transform.rotation = body.transform.rotation;
        body.transform.GetChild(0).gameObject.SetActive(false);
        dummy.GetComponent<FallApart>().Activate();
    }


}
