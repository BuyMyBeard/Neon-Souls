using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum ControlsType { MouseAndKeyboard, Gamepad }
public interface IControlsChange
{
    public void OnControlsChanged(ControlsType controlsType);
}
