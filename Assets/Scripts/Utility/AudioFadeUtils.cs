using System.Collections;
using UnityEngine;

public static class AudioFadeUtils
{
    public static IEnumerator FadeOut(AudioSource audioSource, float fadeSpeed, bool setToZero = false)
    {
        while (audioSource.volume > 0)
        {
            audioSource.volume -= Time.deltaTime * fadeSpeed;
            yield return null;
        }
        audioSource.volume = 0;
        audioSource.Pause();
        if (setToZero)
            audioSource.timeSamples = 0;
    }
    public static IEnumerator FadeIn(AudioSource audioSource, float fadeSpeed, float maxVolume = 1.0f)
    {
        audioSource.UnPause();
        while (audioSource.volume < maxVolume)
        {
            audioSource.volume += Time.deltaTime * fadeSpeed;
            yield return null;
        }
        audioSource.volume = maxVolume;
    }
}