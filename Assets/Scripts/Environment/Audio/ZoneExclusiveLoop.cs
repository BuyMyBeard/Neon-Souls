using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class ZoneExclusiveLoop : MonoBehaviour
{
    [SerializeField] Zone zone;
    public Zone Zone => zone;
    [SerializeField] float fadeSpeed = .5f;
    float maxVolume;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        maxVolume = audioSource.volume;
    }
    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(AudioFadeUtils.FadeOut(audioSource, fadeSpeed));
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(AudioFadeUtils.FadeIn(audioSource, fadeSpeed, maxVolume));
    }
    public void Stop()
    {
        StopAllCoroutines();
        audioSource.volume = 0.0f;
        audioSource.timeSamples = 0;
        audioSource.Stop();
    }
}
