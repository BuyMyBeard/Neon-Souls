using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TransactionNumber : MonoBehaviour
{
    PlayerExperience playerXp;
    private void Awake()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
    }
    public void OnXpChange()
    {
        playerXp.UpdateDisplay();
    }
}
