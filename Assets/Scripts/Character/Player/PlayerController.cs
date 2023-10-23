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
    public bool BlockInput { get; private set; } = false;

    public string CurrentControlScheme { get => playerInput.currentControlScheme; }
    public bool KeyboardAndMouseActive { get => CurrentControlScheme == "Keyboard&Mouse"; }
    public bool GamepadActive { get => CurrentControlScheme == "Gamepad"; }

    InputAction runAction, dodgeAction, parryAction, blockAction;

    PlayerInput playerInput;
    MenuManager menuManager;

    public void SwitchToPlayerControls() => playerInput.SwitchCurrentActionMap("PlayerControls");
    public void SwitchToUI() => playerInput.SwitchCurrentActionMap("UI");
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        menuManager = FindObjectOfType<MenuManager>();
        runAction = playerInput.currentActionMap.FindAction("RunButton");
        dodgeAction = playerInput.currentActionMap.FindAction("DodgeButton");
        parryAction = playerInput.currentActionMap.FindAction("ParryButton");
        blockAction = playerInput.currentActionMap.FindAction("BlockButton");
    }
    private void Start()
    {
        dodgeAction.performed += Dodge_performed;
        runAction.performed += Run_performed;
        runAction.canceled += Run_canceled;
        parryAction.started += Parry_started;
        blockAction.canceled += Block_canceled;
        menuManager.Resume();
    }
    private void Block_canceled(InputAction.CallbackContext obj)
    {
        BlockInput = false;
    }

    private void Parry_started(InputAction.CallbackContext obj)
    {
        BlockInput = true;
        gameObject.SendMessage("OnParry");
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

