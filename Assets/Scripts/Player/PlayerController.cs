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

    public string CurrentControlScheme { get => playerInput.currentControlScheme; }
    public bool KeyboardAndMouseActive { get => CurrentControlScheme == "Keyboard&Mouse"; }
    public bool GamepadActive { get => CurrentControlScheme == "Gamepad"; }

    PlayerInput playerInput;
    EnemyHealthbar enemyHealthbar;
    
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        enemyHealthbar = FindObjectsOfType<EnemyHealthbar>().First(e => e.transform.CompareTag("EnemyHealthbar"));
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
        enemyHealthbar.Remove(5, 50, true);
    }
    void OnHeavyAttack()
    {
        enemyHealthbar.Remove(15, 50, true);
    }
    void OnDodge()
    {
        enemyHealthbar.Add(7, 50);
    }
    void OnInteract()
    {
        enemyHealthbar.Add(18, 50);
    }
    void OnConsumable()
    {
        
    }
    void OnControlsChanged()
    {
        // TODO: Change button prompts
    }
}
