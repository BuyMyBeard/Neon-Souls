using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.UI;
using UnityEngine.SceneManagement;
using UnityEngine.UI;



public class XpMenuManager : MonoBehaviour
{
    [SerializeField] Selectable firstSelected;
    Canvas menuDisplay;
    PlayerController playerController;
    ButtonXp[] buttons;
    XpManager xpManager;
    MenuManager menuManager;
    public bool Active { get => menuDisplay.gameObject.activeSelf; private set => menuDisplay.gameObject.SetActive(value); }

    private void Awake()
    {
        menuDisplay = GetComponentInChildren<Canvas>();
        buttons = FindObjectsOfType<ButtonXp>();
        playerController = FindObjectOfType<PlayerController>();
        xpManager = GetComponent<XpManager>();
        menuManager = FindObjectOfType<MenuManager>();
    }
    void Start()
    {
        Hide();
    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        menuManager.ResetOverride();
        playerController.SwitchToPlayerControls();
        foreach (ButtonXp button in buttons)
        {
            button.ChangeColor(Color.white);
            button.ResetDefault();
        }
        Time.timeScale = 1f;
        Active = false;
    }
    [ContextMenu("Show")]
    public void Show()
    {
        menuManager.OverrideFirstSelected(firstSelected);
        playerController.SwitchToUI();
        Active = true;
        xpManager.RefreshXPAmountRender();
        Time.timeScale = 0f;
    }
}
