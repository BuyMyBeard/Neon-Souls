using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour, IXpReceiver
{
    [SerializeField] string soulsTextTag = "SoulsCount";
    int xpAmount = 0;
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

    GameObject IXpReceiver.gameObject { get => this.gameObject;}

    public void GainXp(int xp)
    { 
        xpAmount += xp;
        xpText.text = xpAmount.ToString();
    }
}
