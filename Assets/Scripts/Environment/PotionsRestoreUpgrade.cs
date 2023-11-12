using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PotionsRestoreUpgrade : Powerup
{
    static int currentLevel = 0;
    [SerializeField] float hdrMultiplier;
    [SerializeField] List<Color> upgradeColors;
    [SerializeField] int potionRestoreBonus;
    Potions potions;
    protected override void Awake()
    {
        base.Awake();
        potions = player.GetComponentInParent<Potions>();
    }

    public override void Apply()
    {
        if (currentLevel >= upgradeColors.Count)
            return;

        base.Apply();

        potions.restoreValue += potionRestoreBonus;
        potions.potionObject.GetComponent<Renderer>().materials[1].SetColor("_EmissionColor", upgradeColors[currentLevel] * hdrMultiplier);
        potions.potionMat.SetColor("_Fluid", upgradeColors[currentLevel]);
        currentLevel++;
    }
}
