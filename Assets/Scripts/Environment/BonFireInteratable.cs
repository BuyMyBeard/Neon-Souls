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
            bonefirerManager.ActivateBonfire(this);
            this.promptMessage = "Sit";
            active = true;
        }
    }
}
