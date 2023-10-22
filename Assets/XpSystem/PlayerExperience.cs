using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour, IXpReceiver
{
    [SerializeField] string soulsTextTag = "SoulsCount";
    [SerializeField]int xpAmount = 0;
    TextMeshProUGUI xpText;
    private void Awake()
    {
        xpText = GameObject.FindGameObjectWithTag(soulsTextTag).GetComponent<TextMeshProUGUI>();
        xpText.text = xpAmount.ToString();
    }
    public int XpAmount
    {
        get { return xpAmount; }
        private set { xpAmount = value; }
    }
    public void GainXp(int amount)
    { 
        xpAmount += amount;
        xpText.text = xpAmount.ToString();
    }

    public void removeXp(int amount)
    {
        xpAmount -= amount;
        xpText.text = xpAmount.ToString();
    }
}
