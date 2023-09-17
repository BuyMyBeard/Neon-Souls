using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;
public class PlayerController : MonoBehaviour
{
    
    public Vector2 Look { get; private set; } = Vector2.zero;
    public Vector2 Move { get; private set; } = Vector2.zero;

    public string CurrentControlScheme { get => playerInput.currentControlScheme; }
    public bool KeyboardAndMouseActive { get => CurrentControlScheme == "Keyboard&Mouse"; }
    public bool GamepadActive { get => CurrentControlScheme == "Gamepad"; }

    PlayerInput playerInput;
    PotionDisplay potions;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        potions = FindObjectOfType<PotionDisplay>();
    }

    void OnMove(InputValue val) => Move = val.Get<Vector2>();
    void OnLook(InputValue val) => Look = val.Get<Vector2>();

    // TODO: Bind these actions to the correct code
    // We could do a priority queue system
    void OnBlock()
    {

    }
    void OnLightAttack()
    {
        potions.Add(3);
    }
    void OnHeavyAttack()
    {
        potions.Clear();
    }
    void OnDodge()
    {
        potions.PotionCount = 2;
    }
    void OnInteract()
    {
        potions.AddOne();
    }
    void OnConsumable()
    {
        potions.RemoveOne();
    }
    void OnControlsChanged()
    {
        // TODO: Change button prompts
    }
}
