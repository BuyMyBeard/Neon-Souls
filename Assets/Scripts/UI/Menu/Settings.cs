using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;
using UnityEngine.UI;
using TMPro;

public class Settings : MonoBehaviour
{
    [SerializeField] Slider masterVolume, sfxVolume, musicVolume, controllerSensX, controllerSensY, mouseSens, colorblindFilterIntensity;
    [SerializeField] Toggle controllerInvertX, controllerInvertY, mouseInvert, vibration;
    [SerializeField] TMP_Dropdown colorblindFilter;
    [SerializeField] AudioMixer audioMixer;
    [SerializeField] Material colorblindnessCorrectionMaterial;
    bool rebindActionDisplayInitialized = false;
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
    public void ChangeColorblindFilter(int index)
    {
        Preferences.ColorblindFilter = index;
        UpdateColorBlindFilter();
    }
    public void ChangeColorblindFilterIntensity(float value)
    {
        Preferences.ColorblindFilterIntensity = value;
        UpdateColorBlindFilterIntensity();
    }
    public void ChangeVibration(bool on)
    {
        Preferences.Vibration = on;
        if (on) Haptics.ExplosionShort();
        else Haptics.Stop();
    }

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
        vibration.isOn = Preferences.Vibration;
        colorblindFilter.value = Preferences.ColorblindFilter;
        colorblindFilterIntensity.value = Preferences.ColorblindFilterIntensity;
        UpdateMaster();
        UpdateSFX();
        UpdateMusic();
        UpdateColorBlindFilter();
        UpdateColorBlindFilterIntensity();
    }
    public void ResetValues()
    {
        Preferences.ResetPlayerPrefs();
        InitiateValues();
        foreach (var rebind in GetComponentsInChildren<IResetableRemap>()) rebind.ResetToDefault();
    }

    void UpdateMaster() => audioMixer.SetFloat("Master", Mathf.Log10(Preferences.MasterVolume) * 20);
    void UpdateSFX() => audioMixer.SetFloat("SFX", Mathf.Log10(Preferences.SFXVolume) * 20);
    void UpdateMusic() => audioMixer.SetFloat("Music", Mathf.Log10(Preferences.MusicVolume) * 20);
    void UpdateColorBlindFilter()
    {
        colorblindnessCorrectionMaterial.SetFloat("_Mode", Preferences.ColorblindFilter);
        colorblindFilterIntensity.interactable = Preferences.ColorblindFilter != 0;
    }
    void UpdateColorBlindFilterIntensity() => colorblindnessCorrectionMaterial.SetFloat("_Intensity", Preferences.ColorblindFilterIntensity);

    public void InitRebindActionDisplay()
    {
        if (rebindActionDisplayInitialized) return;
        foreach (var r in GetComponentsInChildren<SelectableRebindAction>())
            r.UpdateBindingDisplay();
        rebindActionDisplayInitialized = true;
    }
}
