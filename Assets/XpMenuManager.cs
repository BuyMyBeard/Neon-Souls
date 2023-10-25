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
    Canvas menuDisplay;

    private void Awake()
    {
        menuDisplay = GetComponentInChildren<Canvas>();
    }
    void Start()
    {
        menuDisplay.enabled = false; 
    }

    public void Show()
    {
        menuDisplay.enabled = true;
        Time.timeScale = 0f;
    }
}
