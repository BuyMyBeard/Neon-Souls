using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFireInteractble : Interactable
{
    BonfirerManager bonefirerManager;
    public bool active = false;

    protected override void Awake()
    {
        base.Awake();
        bonefirerManager = FindObjectOfType<BonfirerManager>();
        this.promptMessage = "Ligth Fire";
    }
    public override void Interact()
    {
        if (active) 
        {
            bonefirerManager.SitAtBonFire(this);
        }
        else
        {
            active = true;
            bonefirerManager.ActivateBonfire(this);
            StartCoroutine(FlickerCollider());
            this.promptMessage = "Sit";
        }
    }
    IEnumerator FlickerCollider()
    {
        GetComponent<Collider>().enabled = false;
        yield return new WaitForFixedUpdate();
        GetComponent<Collider>().enabled = true;
    }
}
