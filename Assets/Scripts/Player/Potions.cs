using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Potions : MonoBehaviour
{

    [SerializeField] Material potionMat;
    [SerializeField] int maxPotions = 3;
    [SerializeField] int currentPotions;
    [SerializeField] int restoreValue = 60;
    Health health;
    public float FillLevel
    {
        get => potionMat.GetFloat("_Height");
        set => potionMat.SetFloat("_Height", value);
    }
    void Awake()
    {
        health = GetComponent<Health>();
        ResetPotions();
    }
    public void DrinkOnePotion()
    {
        if (currentPotions > 0)
        {
            currentPotions--;
            UpdateFillLevel();
            health.Heal(restoreValue);
        }
    }
    public void ResetPotions()
    {
        currentPotions = maxPotions;
        UpdateFillLevel();
    }
    void UpdateFillLevel() => FillLevel = (float) currentPotions / maxPotions;
    void OnConsumable()
    {
        DrinkOnePotion();
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
