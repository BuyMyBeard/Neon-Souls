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
    InputInterface inputInterface;
    ButtonXp[] buttons;
    XpManager xpManager;
    MenuManager menuManager;
    public bool Active { get => menuDisplay.gameObject.activeSelf; private set => menuDisplay.gameObject.SetActive(value); }

    private void Awake()
    {
        menuDisplay = GetComponentInChildren<Canvas>();
        buttons = FindObjectsOfType<ButtonXp>();
        inputInterface = FindObjectOfType<InputInterface>();
        xpManager = GetComponent<XpManager>();
        menuManager = FindObjectOfType<MenuManager>();
    }
    void Start()
    {
        Hide();
        ResetAffichage();
    }
    [ContextMenu("Hide")]
    public void Hide()
    {
        foreach (ButtonXp button in buttons)
        {
            button.ChangeColor(Color.white);
        }
        menuManager.Paused = false;
        menuManager.ResetOverride();
        inputInterface.SwitchToPlayerControls();
        Time.timeScale = 1f;
        Active = false;
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
    [ContextMenu("Show")]
    public void Show()
    {
        menuManager.Paused = true;
        menuManager.OverrideFirstSelected(firstSelected);
        inputInterface.SwitchToUI();
        Active = true;
        xpManager.RefreshXPAmountRender();
        Time.timeScale = 0f;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        xpManager.ResetXpManager();
    }
    [ContextMenu("Reset")]
    public void ResetAffichage()
    {
        foreach (ButtonXp button in buttons)
        {
            button.ResetDefault();
            button.ResetShownedValue();
        }
        xpManager.ResetXpManager();
    }
}
