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
    Animator animator;
    TextMeshProUGUI transactionText;
    TextMeshProUGUI xpText;
    int transactionSum = 0;
    private void Awake()
    {
        xpText = GameObject.FindGameObjectWithTag(soulsTextTag).GetComponent<TextMeshProUGUI>();
        transactionText = GameObject.FindGameObjectWithTag(transactionTextTag).GetComponent<TextMeshProUGUI>();
        animator = transactionText.GetComponent<Animator>();
        xpText.text = xpAmount.ToString();
    }
    public int XpAmount => xpAmount; 
    
    public void GainXp(int amount)
    { 
        transactionSum += amount;
        xpAmount += amount;
        transactionText.text = (Mathf.Sign(transactionSum) == 1 ? "+" : "") + transactionSum.ToString();
        animator.SetTrigger("Display");
    }
    public void UpdateDisplay()
    {
        transactionSum = 0;
        xpText.text = xpAmount.ToString();
    }

    public void RemoveXp(int amount)
    {
        transactionSum -= amount;
        xpAmount -= amount;
        transactionText.text = (Mathf.Sign(transactionSum) == 1 ? "+" : "") + transactionSum.ToString();
        animator.SetTrigger("Display");
    }
}
