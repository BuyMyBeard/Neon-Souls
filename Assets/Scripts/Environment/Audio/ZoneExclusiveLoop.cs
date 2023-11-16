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
    }
    public void StartFadeOut()
    {
        StopAllCoroutines();
        StartCoroutine(FadeOut());
    }

    public void StartFadeIn()
    {
        StopAllCoroutines();
        StartCoroutine(FadeIn());
    }

    IEnumerator FadeOut()
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Pause();
    }
    IEnumerator FadeIn()
    {
        audioSource.UnPause();
        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        audioSource.volume = maxVolume;
    }
    private void OnValidate()
    {
        maxVolume = GetComponent<AudioSource>().volume;
    }
}
