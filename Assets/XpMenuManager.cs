using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class XpMenuManager : MonoBehaviour
{a
    Canvas menuDisplay;
    PlayerController playerController;
    ButtonXp[] buttons;

    private void Awake()
    {
        menuDisplay = GetComponentInChildren<Canvas>();
        buttons = FindObjectsOfType<ButtonXp>();
        playerController = FindObjectOfType<PlayerController>();
    }
    void Start()
    {
        Hide();
    }
    public void Hide()
    {
        playerController.SwitchToPlayerControls();
        menuDisplay.enabled = false;
        foreach (ButtonXp button in buttons)
        {
            button.ChangeColor(Color.white);
        }
        Time.timeScale = 1f;
    }
    public void Show()
    {
        playerController.SwitchToUI();
        menuDisplay.enabled = true;
        Time.timeScale = 0f;
    }
}
