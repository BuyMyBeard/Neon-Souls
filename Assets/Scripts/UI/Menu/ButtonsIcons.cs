using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public enum SupportedDevices { NotSupported, Gamepad, Keyboard }
/// <summary>
/// This is an example for how to override the default display behavior of bindings. The component
/// hooks into <see cref="RebindActionUI.updateBindingUIEvent"/> which is triggered when UI display
/// of a binding should be refreshed. It then checks whether we have an icon for the current binding
/// and if so, replaces the default text display with an icon.
/// </summary>

public class ButtonsIcons : MonoBehaviour
{
    public GamepadIcons xbox;
    public GamepadIcons ps4;
    public KeyboardIcons keyboard;
    [SerializeField] List<PromptUpdate> promptUpdates;
    [Serializable]
    struct PromptUpdate
    {
        public SelectableRebindAction binding;
        public Image image;
        public SupportedDevices supportedDevice;
    }

    protected void Awake()
    {
        // Hook into all updateBindingUIEvents on all RebindActionUI components in our hierarchy.
        var rebindUIComponents = transform.GetComponentsInChildren<SelectableRebindAction>();
        foreach (var component in rebindUIComponents)
        {
            component.updateBindingUIEvent.AddListener(OnUpdateBindingDisplay);
            component.UpdateBindingDisplay();
        }
    }

    protected void OnUpdateBindingDisplay(SelectableRebindAction component, string bindingDisplayString, string deviceLayoutName, string controlPath)
    {
        if (string.IsNullOrEmpty(deviceLayoutName) || string.IsNullOrEmpty(controlPath))
            return;

        var icon = default(Sprite);
        SupportedDevices device = default;
        if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "DualShockGamepad"))
        {
            icon = ps4.GetSprite(controlPath);
            device = SupportedDevices.Gamepad;

        }
        else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Gamepad"))
        {
            icon = xbox.GetSprite(controlPath);
            device = SupportedDevices.Gamepad;
        }
        else if (InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Keyboard") || InputSystem.IsFirstLayoutBasedOnSecond(deviceLayoutName, "Mouse"))
        {
            icon = keyboard.GetSprite(controlPath);
            device = SupportedDevices.Keyboard;

        }

        var textComponent = component.bindingText;


        // Grab Image component.
        var imageGO = textComponent.transform.parent.Find("ActionBindingIcon");
        var imageComponent = imageGO.GetComponent<Image>();

        if (icon != null)
        {
            textComponent.gameObject.SetActive(false);
            imageComponent.sprite = icon;
            imageComponent.gameObject.SetActive(true);
            int index = promptUpdates.FindIndex((promptUpdate => promptUpdate.binding == component && device == promptUpdate.supportedDevice));
            if (index != -1)
                promptUpdates[index].image.sprite = icon;
        }
        else
        {
            textComponent.gameObject.SetActive(true);
            imageComponent.gameObject.SetActive(false);
        }
    }

    [Serializable]
    public struct GamepadIcons
    {
        public Sprite buttonSouth;
        public Sprite buttonNorth;
        public Sprite buttonEast;
        public Sprite buttonWest;
        public Sprite startButton;
        public Sprite selectButton;
        public Sprite leftTrigger;
        public Sprite rightTrigger;
        public Sprite leftShoulder;
        public Sprite rightShoulder;
        public Sprite dpad;
        public Sprite dpadUp;
        public Sprite dpadDown;
        public Sprite dpadLeft;
        public Sprite dpadRight;
        public Sprite leftStick;
        public Sprite rightStick;
        public Sprite leftStickPress;
        public Sprite rightStickPress;

        public Sprite GetSprite(string controlPath)
        {
            // From the input system, we get the path of the control on device. So we can just
            // map from that to the sprites we have for gamepads.
            switch (controlPath)
            {
                case "buttonSouth": return buttonSouth;
                case "buttonNorth": return buttonNorth;
                case "buttonEast": return buttonEast;
                case "buttonWest": return buttonWest;
                case "start": return startButton;
                case "select": return selectButton;
                case "leftTrigger": return leftTrigger;
                case "rightTrigger": return rightTrigger;
                case "leftShoulder": return leftShoulder;
                case "rightShoulder": return rightShoulder;
                case "dpad": return dpad;
                case "dpad/up": return dpadUp;
                case "dpad/down": return dpadDown;
                case "dpad/left": return dpadLeft;
                case "dpad/right": return dpadRight;
                case "leftStick": return leftStick;
                case "rightStick": return rightStick;
                case "leftStickPress": return leftStickPress;
                case "rightStickPress": return rightStickPress;
                default:
                    break;
            }
            return null;
        }
    }
    [Serializable]
    public struct KeyboardIcons
    {
        public Sprite q;
        public Sprite w;
        public Sprite e;
        public Sprite r;
        public Sprite t;
        public Sprite y;
        public Sprite u;
        public Sprite i;
        public Sprite o;
        public Sprite p;
        public Sprite a;
        public Sprite s;
        public Sprite d;
        public Sprite f;
        public Sprite g;
        public Sprite h;
        public Sprite j;
        public Sprite k;
        public Sprite l;
        public Sprite z;
        public Sprite x;
        public Sprite c;
        public Sprite v;
        public Sprite b;
        public Sprite n;
        public Sprite m;
        public Sprite zero;
        public Sprite one;
        public Sprite two;
        public Sprite three;
        public Sprite four;
        public Sprite five;
        public Sprite six;
        public Sprite seven;
        public Sprite eight;
        public Sprite nine;
        public Sprite arrowUp;
        public Sprite arrowDown;
        public Sprite arrowLeft;
        public Sprite arrowRight;
        public Sprite asterisk;
        public Sprite backspace;
        public Sprite BracketL;
        public Sprite BracketR;
        public Sprite ctrl;
        public Sprite del;
        public Sprite enter;
        public Sprite end;
        public Sprite home;
        public Sprite space;
        public Sprite semicolon;
        public Sprite shift;
        public Sprite insert;
        public Sprite plus;
        public Sprite minus;
        public Sprite tilda;
        public Sprite alt;
        public Sprite markLeft;
        public Sprite markRight;
        public Sprite slash;
        public Sprite quote;
        public Sprite windows;
        public Sprite command;
        public Sprite capsLock;
        public Sprite f1;
        public Sprite f2;
        public Sprite f3;
        public Sprite f4;
        public Sprite f5;
        public Sprite f6;
        public Sprite f7;
        public Sprite f8;
        public Sprite f9;
        public Sprite f10;
        public Sprite f11;
        public Sprite f12;
        public Sprite lmb;
        public Sprite rmb;
        public Sprite mmb;
        public Sprite tab;
        public Sprite GetSprite(string controlPath)
        {
            controlPath = controlPath.ToLower();

            switch (controlPath)
            {
                case "q": return q;
                case "w": return w;
                case "e": return e;
                case "r": return r;
                case "t": return t;
                case "y": return y;
                case "u": return u;
                case "i": return i;
                case "o": return o;
                case "p": return p;
                case "a": return a;
                case "s": return s;
                case "d": return d;
                case "f": return f;
                case "g": return g;
                case "h": return h;
                case "j": return j;
                case "k": return k;
                case "l": return l;
                case "z": return z;
                case "x": return x;
                case "c": return c;
                case "v": return v;
                case "b": return b;
                case "n": return n;
                case "m": return m;
                case "0": return zero;
                case "1": return one;
                case "2": return two;
                case "3": return three;
                case "4": return four;
                case "5": return five;
                case "6": return six;
                case "7": return seven;
                case "8": return eight;
                case "9": return nine;
                case "f1": return f1;
                case "f2": return f2;
                case "f3": return f3;
                case "f4": return f4;
                case "f5": return f5;
                case "f6": return f6;
                case "f7": return f7;
                case "f8": return f8;
                case "f9": return f9;
                case "f10": return f10;
                case "f11": return f11;
                case "f12": return f12;
                case "leftarrow": return arrowLeft;
                case "rightarrow": return arrowRight;
                case "downarrow": return arrowDown;
                case "uparrow": return arrowUp;
                case "space": return space;
                case "leftbutton": return lmb;
                case "rightbutton": return rmb;
                case "middlebutton": return mmb;
                case "shift": return shift;
                case "leftshift": return shift;
                case "rightshift": return shift;
                case "backslash": return slash;
                case "quote": return quote;
                case "leftbracket": return BracketL;
                case "rightbracket": return BracketR;
                case "semicolon": return semicolon;
                case "tilda": return tilda;
                case "minus": return minus;
                case "plus": return plus;
                case "numpadplus": return plus;
                case "ctrl": return ctrl;
                case "leftctrl": return ctrl;
                case "rightctrl": return ctrl;
                case "alt": return alt;
                case "leftalt": return alt;
                case "rightalt": return alt;
                case "insert": return insert;
                case "delete": return del;
                case "home": return home;
                case "end": return end;
                case "backspace": return backspace;
                case "leftmeta": return windows;
                case "rightmeta": return windows;
                case "enter": return enter;
                case "capslock": return capsLock;
                case "tab": return tab;
                default:
                    break;
            }
            return null;
        }
    }
}
