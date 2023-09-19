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
    PotionDisplay potions;
    EnemyHealthbar enemyHealthbar;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
        potions = FindObjectOfType<PotionDisplay>();
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
        potions.Add(3);
        enemyHealthbar.Remove(5, 50);
    }
    void OnHeavyAttack()
    {
        potions.Clear();
        enemyHealthbar.Remove(15, 50);
    }
    void OnDodge()
    {
        potions.PotionCount = 2;
        enemyHealthbar.Add(7.5f, 50);
    }
    void OnInteract()
    {
        potions.AddOne();
        enemyHealthbar.Add(18.9999f, 50);
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
