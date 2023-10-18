using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    BonfireManager bonfireManager;
    public bool active = false;
    [SerializeField] Vector3 respawnOffset = Vector3.zero;
    new Light light;

    public Vector3 RespawnOffset { get => respawnOffset; }
    protected override void Awake()
    {
        base.Awake();
        bonfireManager = FindObjectOfType<BonfireManager>();
        light = GetComponent<Light>();
    }
    public override void Interact()
    {
        if (active) 
        {
            bonfireManager.SitAtBonfire(this);
            return;
        }
        light.color = new Color(0, 1, 0);
        active = true;
        bonfireManager.ActivateBonfire(this);
        StartCoroutine(FlickerCollider());
        promptMessage = "Play";
    }
    IEnumerator FlickerCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForFixedUpdate();
        GetComponent<Collider>().enabled = true;
    }
}
