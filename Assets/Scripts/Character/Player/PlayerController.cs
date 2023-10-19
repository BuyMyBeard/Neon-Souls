using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
public class PlayerController : MonoBehaviour
{
    public Vector2 Look { get; private set; } = Vector2.zero;
    public Vector2 Move { get; private set; } = Vector2.zero;
    public bool IsSprinting { get; private set; } = false;
    public bool IsBlocking { get; private set; } = false;

    public string CurrentControlScheme { get => playerInput.currentControlScheme; }
    public bool KeyboardAndMouseActive { get => CurrentControlScheme == "Keyboard&Mouse"; }
    public bool GamepadActive { get => CurrentControlScheme == "Gamepad"; }

    InputAction run, dodge, parry;

    PlayerInput playerInput;
    MenuManager menuManager;

    public void SwitchToPlayerControls() => playerInput.SwitchCurrentActionMap("PlayerControls");
    public void SwitchToUI() => playerInput.SwitchCurrentActionMap("UI");
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        menuManager = FindObjectOfType<MenuManager>();
        run = playerInput.currentActionMap.FindAction("RunButton");
        dodge = playerInput.currentActionMap.FindAction("DodgeButton");
        parry = playerInput.currentActionMap.FindAction("Parry");
    }
    private void Start()
    {
        dodge.performed += Dodge_performed;
        run.performed += Run_performed;
        run.canceled += Run_canceled;
        parry.started += Parry_started;
        parry.canceled += Parry_canceled;
        menuManager.Resume();
    }
    private void Parry_canceled(InputAction.CallbackContext obj)
    {
        IsBlocking = false;
    }

    private void Parry_started(InputAction.CallbackContext obj)
    {
        IsBlocking = true;
    }

    private void Run_canceled(InputAction.CallbackContext obj)
    {
        IsSprinting = false;
    }
    // Necessary because this is a hold interaction
    private void Run_performed(InputAction.CallbackContext obj)
    {
        IsSprinting = true;
    }

    private void Dodge_performed(InputAction.CallbackContext obj)
    {
        SendMessage("OnDodge");
    }
    void OnPlayerPause()
    {
        if (menuManager.Paused) menuManager.Resume();
        else menuManager.Pause();
    }

    void OnUIPause()
    {
        if (menuManager.Paused) menuManager.Resume();
        else menuManager.Pause();
    }
    void OnDodge()
    {
        Debug.Log("dodged");
    }
    void OnRun()
    {
        Debug.Log("ran");
    }

    void OnMove(InputValue val) => Move = val.Get<Vector2>();
    void OnLook(InputValue val) => Look = val.Get<Vector2>();
    void OnControlsChanged()
    {
        // TODO: Change button prompts
    }
}

