using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TextMeshProUGUI))]
public class XpAmountText : MonoBehaviour
{
    TextMeshProUGUI textMeshProUGUI;
    PlayerExperience playerXp;
    private void Awake()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        textMeshProUGUI = GetComponent<TextMeshProUGUI>();
    }
    public void RefreshRender(string textToShow)
    {
        textMeshProUGUI.text = textToShow;
    }
}
