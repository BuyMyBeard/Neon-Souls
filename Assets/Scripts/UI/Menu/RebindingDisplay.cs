using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class RebindingDisplay : MonoBehaviour
{
    [SerializeField] InputActionReference action;
    [SerializeField] EventSystem eventSystem;
}
