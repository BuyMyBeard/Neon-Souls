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
    }
    public override void Interact()
    {
        if (active) 
        {
            bonefirerManager.SetSpawningBoneFire(this);
        }
        else
        {
            bonefirerManager.ActivateBonfire(this);
            this.promptMessage = "Set Spawnpoint to this bonfire";
            active = true;
        }
    }
}
