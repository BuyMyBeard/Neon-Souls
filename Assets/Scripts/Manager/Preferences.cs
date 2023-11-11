using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Preferences
{
    static private float musicVolume, sfxVolume, masterVolume, controllerSensX, controllerSensY, mouseSens, colorblindFilterIntensity;
    static private int controllerInvertX, controllerInvertY, mouseInvert, vibration, colorblindFilter;
    static public float MasterVolume
    {
        get => masterVolume;
        set
        {
            masterVolume = value;
            PlayerPrefs.SetFloat("MasterVolume", value);
        }
    }
    static public float MusicVolume
    {
        get => musicVolume;
        set
        {
            musicVolume = value;
            PlayerPrefs.SetFloat("MusicVolume", value);
        }
    }
    static public float SFXVolume
    {
        get => sfxVolume;
        set
        {
            sfxVolume = value;
            PlayerPrefs.SetFloat("SFXVolume", value);
        }
    }
    static public bool ControllerInvertX
    {
        get => controllerInvertX == 1;
        set
        {
            int intVal = value ? 1 : 0;
            controllerInvertX = intVal;
            PlayerPrefs.SetInt("ControllerInvertX", intVal);
        }
    }
    static public bool ControllerInvertY
    {
        get => controllerInvertY == 1;
        set
        {
            int intVal = value ? 1 : 0;
            controllerInvertY = intVal;
            PlayerPrefs.SetInt("ControllerInvertY", intVal);
        }
    }
    static public bool MouseInvert
    {
        get => mouseInvert == 1;
        set
        {
            int intVal = value ? 1 : 0;
            mouseInvert = intVal;
            PlayerPrefs.SetInt("MouseInvert", intVal);
        }
    }
    static public bool Vibration
    {
        get => vibration == 1;
        set
        {
            int intVal = value ? 1 : 0;
            vibration = intVal;
            PlayerPrefs.SetInt("Vibration", intVal);
        }
    }
    static public float ControllerSensitivityX
    {
        get => controllerSensX;
        set
        {
            controllerSensX = value;
            PlayerPrefs.SetFloat("ControllerSensX", value);
        }
    }
    static public float ControllerSensivityY
    {
        get => controllerSensY;
        set
        {
            controllerSensY = value;
            PlayerPrefs.SetFloat("ControllerSensY", value);
        }
    }
    static public float MouseSensitivity
    {
        get => mouseSens;
        set
        {
            mouseSens = value;
            PlayerPrefs.SetFloat("MouseSens", value);
        }
    }
    static public int ColorblindFilter
    {
        get => colorblindFilter;
        set
        {
            colorblindFilter = value;
            PlayerPrefs.SetInt("ColorblindFilter", value);
        }
    }
    static public float ColorblindFilterIntensity
    {
        get => colorblindFilterIntensity;
        set
        {
            colorblindFilterIntensity = value;
            PlayerPrefs.SetFloat("ColorblindFilterIntensity", value);
        }
    }

    static Preferences()
    {
        LoadPlayerPrefs();
    }
    static void LoadPlayerPrefs()
    {
        MasterVolume = PlayerPrefs.GetFloat("MasterVolume", 1f);
        MusicVolume = PlayerPrefs.GetFloat("MusicVolume", 1f);
        SFXVolume = PlayerPrefs.GetFloat("SFXVolume", 1f);
        ControllerInvertX = PlayerPrefs.GetInt("ControllerInvertX", 0) == 1;
        ControllerInvertY = PlayerPrefs.GetInt("ControllerInvertY", 0) == 1;
        MouseInvert = PlayerPrefs.GetInt("MouseInvert", 0) == 1;
        ControllerSensitivityX = PlayerPrefs.GetFloat("ControllerSensX", .7f);
        ControllerSensivityY = PlayerPrefs.GetFloat("ControllerSensY", .7f);
        MouseSensitivity = PlayerPrefs.GetFloat("MouseSens", .2f);
        Vibration = PlayerPrefs.GetInt("Vibration", 1) == 1;
        ColorblindFilter = PlayerPrefs.GetInt("ColorblindFilter", 0);
        ColorblindFilterIntensity = PlayerPrefs.GetFloat("ColorblindFilterIntensity", 1f);
    }
    static public void ResetPlayerPrefs()
    {
        PlayerPrefs.DeleteAll();
        LoadPlayerPrefs();
    }
}
