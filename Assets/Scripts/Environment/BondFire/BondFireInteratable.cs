using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoneFireInteratable : Interactable
{
    BonefirerManager bonefirerManager;
    public bool active = false;

    protected override void Awake()
    {
        base.Awake();
        bonefirerManager = FindObjectOfType<BonefirerManager>();
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
            active = true;
        }
    }
}
