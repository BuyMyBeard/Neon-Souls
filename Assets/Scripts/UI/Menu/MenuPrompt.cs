using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPrompt : MonoBehaviour, IControlsChange
{
    [SerializeField] Sprite controllerButton;
    [SerializeField] Sprite keyboardButton;
    Image buttonImage;
    private void Awake()
    {
        buttonImage = GetComponent<Image>();
    }
    public void OnControlsChanged(ControlsType controlsType)
    {
        if (controlsType == ControlsType.MouseAndKeyboard)
            buttonImage.sprite = keyboardButton;
        else if (controlsType == ControlsType.Gamepad)
            buttonImage.sprite = controllerButton;
    }
}
