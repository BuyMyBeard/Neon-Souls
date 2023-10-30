using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour, IRechargeable
{
    GameObject model;
    GameObject character;

    private void Awake()
    {
        character = GetComponentInChildren<Animator>().gameObject;
        model = character.transform.GetChild(0).gameObject;
    }
    [ContextMenu("Decompose")]
    public void Decompose()
    {
        GameObject dummy = Instantiate(character);
        dummy.transform.SetPositionAndRotation(character.transform.position, character.transform.rotation);
        model.SetActive(false);
        dummy.GetComponent<FallApart>().Activate();
    }

    public void Recharge()
    {
        model.SetActive(true);
    }
}
