using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Rendering;
using UnityEngine.Windows;
public class InputInterface : MonoBehaviour
{
    public Vector2 Look { get; private set; } = Vector2.zero;
    public Vector2 Move { get; private set; } = Vector2.zero;
    public bool IsSprinting { get; private set; } = false;
    public bool BlockInput { get; private set; } = false;

    public bool PausedThisFrame { get; private set; } = false;
    public string CurrentControlScheme { get => playerInput.currentControlScheme; }
    public bool KeyboardAndMouseActive { get => CurrentControlScheme == "Keyboard&Mouse"; }
    public bool GamepadActive { get => CurrentControlScheme == "Gamepad"; }

    InputAction runAction, dodgeAction, parryAction, blockAction;

    PlayerInput playerInput;
    MenuManager menuManager;
    XpMenuManager xpMenuManager;
    BossManager bossManager;

    bool ignoreDodgeInput = false;

    List<IControlsChangedListener> controlsChangedListeners;
    public void SwitchToPlayerControls() => playerInput.SwitchCurrentActionMap("PlayerControls");
    public void SwitchToUI() => playerInput.SwitchCurrentActionMap("UI");
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        menuManager = FindObjectOfType<MenuManager>();
        xpMenuManager = FindObjectOfType<XpMenuManager>();
        bossManager = FindObjectOfType<BossManager>();
        runAction = playerInput.currentActionMap.FindAction("RunButton");
        dodgeAction = playerInput.currentActionMap.FindAction("DodgeButton");
        parryAction = playerInput.currentActionMap.FindAction("ParryButton");
        blockAction = playerInput.currentActionMap.FindAction("BlockButton");
    }
    private void OnEnable()
    {
        dodgeAction.performed += Dodge_performed;
        runAction.performed += Run_performed;
        runAction.canceled += Run_canceled;
        parryAction.started += Parry_started;
        blockAction.canceled += Block_canceled;
    }

    private void OnDisable()
    {
        dodgeAction.performed -= Dodge_performed;
        runAction.performed -= Run_performed;
        runAction.canceled -= Run_canceled;
        parryAction.started -= Parry_started;
        blockAction.canceled -= Block_canceled;
    }
    private void Block_canceled(InputAction.CallbackContext obj)
    {
        BlockInput = false;
    }

    private void Parry_started(InputAction.CallbackContext obj)
    {
        if (PausedThisFrame) return;
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
        if (ignoreDodgeInput)
            ignoreDodgeInput = false;
        else
            SendMessage("OnDodge");
    }
    void OnPlayerPause()
    {
        if (PausedThisFrame) return;
        if (bossManager != null && bossManager.CutsceneInProgress)
            Time.timeScale = 10;
        else
            StartCoroutine(Pause());
    }


    IEnumerator Pause()
    {
        PausedThisFrame = true;
        ignoreDodgeInput = true;
        yield return null;
        if (menuManager.Paused) menuManager.Resume();
        else menuManager.Pause();
        yield return null;
        PausedThisFrame = false;

        // This is so fucking dumb but I have to do it, there is no other way
        yield return new WaitForSecondsRealtime(.05f);
        ignoreDodgeInput = false;
    }
    IEnumerator HideXpMenu()
    {
        PausedThisFrame = true;
        ignoreDodgeInput = true;
        yield return null;
        xpMenuManager.Hide();
        xpMenuManager.ResetAffichage();
        yield return null;
        PausedThisFrame = false;

        yield return new WaitForSecondsRealtime(1);
        ignoreDodgeInput = false;
    }

    public void OnUIPause()
    {
        if (PausedThisFrame) return;
        if (xpMenuManager.Active)
            StartCoroutine(HideXpMenu());
        else
            StartCoroutine(Pause());
    }

    void OnMove(InputValue val) => Move = val.Get<Vector2>();
    void OnLook(InputValue val) => Look = val.Get<Vector2>();

    void OnControlsChanged()
    {
        //Because this fucking garbage gets called before the thing even awakens
        playerInput ??= GetComponent<PlayerInput>();
        controlsChangedListeners ??= FindObjectsOfType<MonoBehaviour>().OfType<IControlsChangedListener>().ToList();
        foreach (IControlsChangedListener listener in controlsChangedListeners)
            listener.ControlsChanged(GamepadActive ? SupportedDevices.Gamepad : SupportedDevices.Keyboard);
    }
}

interface IControlsChangedListener
{
    public void ControlsChanged(SupportedDevices device);
}

