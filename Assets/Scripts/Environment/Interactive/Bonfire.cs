using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Light))]
[RequireComponent(typeof(Sounds))]
public class Bonfire : Interactable
{
    XpMenuManager xpMenuManager;
    BonfireManager bonfireManager;
    public bool active = false;
    [SerializeField] Transform respawnPosition;
    new Light light;
    [SerializeField] Zone zone;
    Sounds sounds;
    
    public Zone Zone => zone;
    public Vector3 RespawnPosition => respawnPosition.position;

    public override string animationTriggerName => "Interact";

    protected override void Awake()
    {
        base.Awake();
        xpMenuManager = FindObjectOfType<XpMenuManager>();
        bonfireManager = FindObjectOfType<BonfireManager>();
        light = GetComponent<Light>();
        sounds = GetComponent<Sounds>();
    }
    private IEnumerator Start()
    {
        yield return null;
        if (active)
            light.color = Color.green;
    }
    public override void Interact()
    {
        sounds.Play(Sound.Kindle, .25f);
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
        promptMessage = "Améliorer";
    }
    IEnumerator FlickerCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForFixedUpdate();
        GetComponent<Collider>().enabled = true;
    }
}
