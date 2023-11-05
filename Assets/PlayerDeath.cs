using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerDeath : MonoBehaviour, IRechargeable
{
    GameObject model;
    GameObject character;
    [SerializeField] GameObject droppedXp;

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
        DroppedXp previousDroppedXp = FindObjectOfType<DroppedXp>();
        if(previousDroppedXp != null ) { Destroy(previousDroppedXp.gameObject); }
        Instantiate(droppedXp, character.transform.position, Quaternion.identity);
    }

    public void Recharge()
    {
        model.SetActive(true);
    }
}
