using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider masterVolume, sfxVolume, musicVolume, controllerSensX, controllerSensY, mouseSens;
    [SerializeField] Toggle controllerInvertX, controllerInvertY, mouseInvert;
    [SerializeField] AudioMixer audioMixer;
    private void Awake()
    {
        InitiateValues();
    }
    public void ChangeMasterVolume(float volume)
    {
        Preferences.MasterVolume = volume;
        UpdateMaster();
    }
    public void ChangeSFXVolume(float volume)
    {
        Preferences.SFXVolume = volume;
        UpdateSFX();
    }
    public void ChangeMusicVolume(float volume)
    {
        Preferences.MusicVolume = volume;
        UpdateMusic();
    }
    public void ChangeControllerSensX(float sens) => Preferences.ControllerSensitivityX = sens;
    public void ChangeControllerSensY(float sens) => Preferences.ControllerSensivityY = sens;
    public void ChangeMouseSens(float sens) => Preferences.MouseSensitivity = sens;
    public void ChangeControllerInvertX(bool inverted) => Preferences.ControllerInvertX = inverted;
    public void ChangeControllerInvertY(bool inverted) => Preferences.ControllerInvertY = inverted;
    public void ChangeMouseInvert(bool inverted) => Preferences.MouseInvert = inverted;

    private void InitiateValues()
    {
        masterVolume.value = Preferences.MasterVolume;
        sfxVolume.value = Preferences.SFXVolume;
        musicVolume.value = Preferences.MusicVolume;
        controllerSensX.value = Preferences.ControllerSensitivityX;
        controllerSensY.value = Preferences.ControllerSensivityY;
        mouseSens.value = Preferences.MouseSensitivity;
        controllerInvertX.isOn = Preferences.ControllerInvertX;
        controllerInvertY.isOn = Preferences.ControllerInvertY;
        mouseInvert.isOn = Preferences.MouseInvert;
        UpdateMaster();
        UpdateSFX();
        UpdateMusic();
    }
    public void ResetValues()
    {
        Preferences.ResetPlayerPrefs();
        InitiateValues();
        foreach (var rebind in GetComponentsInChildren<SelectableRebindAction>())
        {
            rebind.ResetToDefault();
        }
    }

    void UpdateMaster() => audioMixer.SetFloat("Master", Mathf.Log10(Preferences.MasterVolume) * 20);
    void UpdateSFX() => audioMixer.SetFloat("SFX", Mathf.Log10(Preferences.SFXVolume) * 20);
    void UpdateMusic() => audioMixer.SetFloat("Music", Mathf.Log10(Preferences.MusicVolume) * 20);
}
