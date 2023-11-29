using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class IncreasedPotionCountUpgrade : Powerup
{
    static Animator plusOneAnimator;
    Potions potions;
    protected override void Awake()
    {
        base.Awake();
        if (plusOneAnimator == null)
            plusOneAnimator = GameObject.FindGameObjectWithTag("PlusOneIcon").GetComponent<Animator>();
        potions = player.GetComponentInParent<Potions>();
    }

    public override void Apply()
    {
        base.Apply();
        potions.maxPotions++;
        potions.currentPotions++;
        potions.UpdateFillLevel();
        plusOneAnimator.SetTrigger("Activate");
    }
}
