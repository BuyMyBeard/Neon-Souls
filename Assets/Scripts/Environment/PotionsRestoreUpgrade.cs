using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PotionsRestoreUpgrade : Powerup
{
    static int currentLevel = 0;
    [SerializeField] float hdrMultiplier;
    [SerializeField] List<Color> upgradeColors;
    [SerializeField] int potionRestoreBonus;
    [SerializeField] Sprite SecondUpgradeSprite;
    Potions potions;
    PotionsRestoreUpgrade[] others;
    protected override void Awake()
    {
        base.Awake();
        potions = player.GetComponentInParent<Potions>();
        currentLevel = 0;
        others = FindObjectsOfType<PotionsRestoreUpgrade>();
    }

    public override void Apply()
    {
        if (currentLevel >= upgradeColors.Count)
            return;

        base.Apply();

        potions.restoreValue += potionRestoreBonus;
        potions.SetLiquidColor(upgradeColors[currentLevel] * hdrMultiplier);
        potions.potionMat.SetColor("_Fluid", upgradeColors[currentLevel]);
        currentLevel++;
    }

    override protected void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);
        foreach (var upgrade in others)
            upgrade.GetComponent<SpriteRenderer>().sprite = SecondUpgradeSprite;
    }
}
