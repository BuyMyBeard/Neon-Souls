using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    BonfireManager bonfireManager;
    public bool active = false;
    [SerializeField] Vector3 respawnOffset = Vector3.zero;

    public Vector3 RespawnOffset { get => respawnOffset; }
    protected override void Awake()
    {
        base.Awake();
        bonfireManager = FindObjectOfType<BonfireManager>();
        promptMessage = "Light Fire";
    }
    public override void Interact()
    {
        if (active) 
        {
            bonfireManager.SitAtBonfire(this);
        }
        else
        {
            active = true;
            bonfireManager.ActivateBonfire(this);
            StartCoroutine(FlickerCollider());
            promptMessage = "Sit";
        }
    }
    IEnumerator FlickerCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForFixedUpdate();
        GetComponent<Collider>().enabled = true;
    }
}
