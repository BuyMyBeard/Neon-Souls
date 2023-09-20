using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{

    [SerializeField] Material potionMat;
    [SerializeField] int maxPotions = 3;
    [SerializeField] int potions;
    [SerializeField] int restoreValue = 60;
    Health health;
    public float FillLevel
    {
        get => potionMat.GetFloat("_Height");
        set => potionMat.SetFloat("_Height", value);
    }
    void Awake()
    {
        health = FindObjectOfType<Health>();
        ResetPotions();
    }
    public void DrinkOnePotion()
    {
        if (potions > 0)
        {
            potions--;
            UpdateFillLevel();
            health.Heal(restoreValue);
        }
    }
    public void ResetPotions()
    {
        potions = maxPotions;
        UpdateFillLevel();
    }
    void UpdateFillLevel() => FillLevel = (float) potions / maxPotions;
    void OnConsumable()
    {
        DrinkOnePotion();
    }
    void OnInteract()
    {
        ResetPotions();
    }
    /*TODO: Implement.
    IEnumerator CatchUp(float goal)
    {
        isCatchingUp = true;
        float start = DisplayedValue;
        for (float t = 0; t <= 1; t += Time.deltaTime * catchUpSpeed)
        {
            yield return null;
            DisplayedValue = Mathf.Lerp(start, goal, t);
        }
        DisplayedValue = goal;
        isCatchingUp = false;
    }*/

}
