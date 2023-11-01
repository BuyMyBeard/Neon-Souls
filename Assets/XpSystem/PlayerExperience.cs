using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PlayerExperience : MonoBehaviour, IXpReceiver
{
    [SerializeField] string soulsTextTag = "SoulsCount";
    [SerializeField] string transactionTextTag = "TransactionText";
    [SerializeField]int xpAmount = 0;
    [SerializeField] Animator transactionAnimator;
    TextMeshProUGUI transactionText;
    TextMeshProUGUI xpText;
    private void Awake()
    {
        xpText = GameObject.FindGameObjectWithTag(soulsTextTag).GetComponent<TextMeshProUGUI>();
        transactionText = GameObject.FindGameObjectWithTag(transactionTextTag).GetComponent<TextMeshProUGUI>();
        xpText.text = xpAmount.ToString();
    }
    public int XpAmount => xpAmount; 
    
    public void GainXp(int amount)
    { 
        xpAmount += amount;
        transactionText.text = "+" + amount.ToString();
        transactionAnimator.Play("SoulsCount");
    }
    public void UpdateDisplay()
    {
        xpText.text = xpAmount.ToString();
    }

    public void removeXp(int amount)
    {
        xpAmount -= amount;
        Debug.Log(amount);
        transactionText.text = "-" + amount.ToString();
        transactionAnimator.Play("SoulsCount");
    }
}
