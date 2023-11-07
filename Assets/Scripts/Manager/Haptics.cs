using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Haptics : MonoBehaviour
{
    // timestamp, intensity
    private readonly List<(float, float)> lowFreqQueue = new();
    private readonly List<(float, float)> highFreqQueue = new();

    float currentLowFreqIntensity = 0;
    float currentHighFreqIntensity = 0;

    bool GamepadConnected => Gamepad.all.Count > 0;

    static private Haptics Instance { get; set; }
    private void Awake()
    {
        Instance = this;
        foreach (Button button in FindObjectsOfType<Button>(true))
        {
            button.onClick.AddListener(InstanceButtonPress);
        }
    }

    IEnumerator Start()
    {
        while (true)
        {
            yield return new WaitUntil(() => Time.timeScale == 0);
            // StopHaptics();
            yield return new WaitWhile(() => Time.timeScale == 0);
            // ResumeHaptics();
        }
    }

    private void Update()
    {
        if (GamepadConnected)
        {
            bool hasChanged = false;
            while (lowFreqQueue.Count > 0)
            {
                if (lowFreqQueue[0].Item1 < Time.unscaledTime)
                    lowFreqQueue.RemoveAt(0);
                else break;
            }
            while (highFreqQueue.Count > 0)
            {
                if (highFreqQueue[0].Item1 < Time.unscaledTime)
                    highFreqQueue.RemoveAt(0);
                else break;
            }
            if (lowFreqQueue.Count > 0)
            {
                float highest = lowFreqQueue.Max(stamp => stamp.Item2);
                if (highest < currentLowFreqIntensity)
                {
                    currentLowFreqIntensity = highest;
                    hasChanged = true;
                }
            }
            else if (currentLowFreqIntensity != 0)
            {
                currentLowFreqIntensity = 0;
                hasChanged = true;
            }
            if (highFreqQueue.Count > 0)
            {
                float highest = highFreqQueue.Max(stamp => stamp.Item2);
                if (highest < currentHighFreqIntensity)
                {
                    currentHighFreqIntensity = highest;
                    hasChanged = true;
                }
            }
            else if (currentHighFreqIntensity != 0)
            {
                currentHighFreqIntensity = 0;
                hasChanged = true;
            }

            if (hasChanged) ResumeHaptics();
        }
    }
    public void Vibrate(float lowFreq, float highFreq, float time)
    {
        VibrateHighFreq(lowFreq, time);
        VibrateLowFreq(highFreq, time);
    }

    public void VibrateHighFreq(float intensity, float time)
    {
        if( GamepadConnected )
        {
            Mathf.Clamp01(intensity);
            if (intensity > currentHighFreqIntensity)
            {
                currentHighFreqIntensity = intensity;
                ResumeHaptics();
            }
            float vibrationExpiration = Time.unscaledTime + time;
            int index = highFreqQueue.FindIndex(stamp => stamp.Item1 < vibrationExpiration);
            if (index == -1) highFreqQueue.Add((vibrationExpiration, intensity));
            else highFreqQueue.Insert(index, (vibrationExpiration, intensity));

        }
    }
    public void VibrateLowFreq(float intensity, float time)
    {
        if ( GamepadConnected )
        {
            Mathf.Clamp01(intensity);
            if (intensity > currentLowFreqIntensity)
            {
                currentLowFreqIntensity = intensity;
                ResumeHaptics();
            }
            float vibrationExpiration = Time.unscaledTime + time;
            int index = lowFreqQueue.FindIndex(stamp => stamp.Item1 < vibrationExpiration);
            if (index == -1) lowFreqQueue.Add((vibrationExpiration, intensity));
            else lowFreqQueue.Insert(index, (vibrationExpiration, intensity));

        }
    }
    public void StopHaptics() => Gamepad.current.SetMotorSpeeds(0, 0);
    private void ResumeHaptics()
    {
        if (Preferences.Vibration) Gamepad.current.SetMotorSpeeds(currentLowFreqIntensity, currentHighFreqIntensity);
    }

    private void OnDestroy()
    {
        if( GamepadConnected )
        {
            StopHaptics();
        }
    }
    public static void ButtonPress() => Instance.InstanceButtonPress();
    public static void ImpactLight() => Instance.InstanceImpactLight();
    public static void Impact() => Instance.InstanceImpact();
    public static void ImpactHeavy() => Instance.InstanceImpactHeavy();
    public static void ExplosionShort() => Instance.InstanceExplosionShort();
    public static void Explosion() => Instance.InstanceExplosion();
    public static void ExplosionLong() => Instance.InstanceExplosionLong();
    public static void AmbientSubtle() => Instance.InstanceAmbientSubtle();
    public static void Ambient() => Instance.InstanceAmbient();
    public static void AmbientStrong() => Instance.InstanceAmbientStrong();
    public static void Stop() => Instance.StopHaptics();

    [ContextMenu("ButtonPress")]
    public void InstanceButtonPress() => Instance.Vibrate(.5f, .5f, .1f);
    [ContextMenu("ImpactLight")]
    public void InstanceImpactLight() => Instance.Vibrate(0, .5f, .2f);
    [ContextMenu("Impact")]
    public void InstanceImpact() => Instance.Vibrate(.2f, .8f, .2f);
    [ContextMenu("ImpactHeavy")]
    public void InstanceImpactHeavy() => Instance.Vibrate(.5f, 1f, .2f);
    [ContextMenu("ExplosionShort")]
    public void InstanceExplosionShort() => Instance.Vibrate(.5f, .25f, .2f);
    [ContextMenu("Explosion")]
    public void InstanceExplosion() => Instance.Vibrate(.8f, .4f, .5f);
    [ContextMenu("ExplosionLong")]
    public void InstanceExplosionLong() => Instance.Vibrate(1f, .5f, 1f);
    [ContextMenu("AmbientSubtle")]
    public void InstanceAmbientSubtle() => Instance.Vibrate(.1f, 0f, 10f);
    [ContextMenu("Ambient")]
    public void InstanceAmbient() => Instance.Vibrate(.3f, .1f, 10f);
    [ContextMenu("AmbientStrong")]
    public void InstanceAmbientStrong() => Instance.Vibrate(.6f, .3f, 10f);
}
