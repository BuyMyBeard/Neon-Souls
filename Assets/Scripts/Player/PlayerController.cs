using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Windows;

public class PlayerController : MonoBehaviour
{
    public Vector2 LookDelta { get; private set; } = Vector2.zero;
    public Vector2 MoveInput { get; private set; } = Vector2.zero;

    PlayerInput playerInput;
    private void Awake()
    {
        playerInput = GetComponent<PlayerInput>();
    }
    private void Update()
    {

    }
    //void OnLook(InputValue val)
    //{
    //    Debug.Log(val.Get<Vector2>());
    //    Debug.Log(playerInput.currentControlScheme);
    //}
    void OnMove(InputValue val)
    {
        MoveInput = val.Get<Vector2>();
    }
    void OnLook(InputValue val)
    {
        LookDelta = val.Get<Vector2>();
    }

}
