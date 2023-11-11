using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bonfire : Interactable
{
    XpMenuManager xpMenuManager;
    BonfireManager bonfireManager;
    public bool active = false;
    [SerializeField] Vector3 respawnOffset = Vector3.zero;
    new Light light;

    public Vector3 RespawnOffset { get => respawnOffset; }

    public override string animationTriggerName => "Interact";

    protected override void Awake()
    {
        base.Awake();
        xpMenuManager = FindObjectOfType<XpMenuManager>();
        bonfireManager = FindObjectOfType<BonfireManager>();
        light = GetComponent<Light>();
    }
    private IEnumerator Start()
    {
        yield return null;
        if (active)
            light.color = Color.green;
    }
    public override void Interact()
    {
        if (active) 
        {
            bonfireManager.SitAtBonfire(this);
            xpMenuManager.Show();
            
            return;
        }
        light.color = new Color(0, 1, 0);
        active = true;
        bonfireManager.ActivateBonfire(this);
        StartCoroutine(FlickerCollider());
        promptMessage = "Jouer à l'arcade";
    }
    IEnumerator FlickerCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForFixedUpdate();
        GetComponent<Collider>().enabled = true;
    }
}
