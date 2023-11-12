using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[RequireComponent(typeof(AudioSource))]

public class Sounds : MonoBehaviour
{
    readonly Dictionary<Sound, AudioSource> soundLoops = new();
    AudioSource soundSource = null;
    private void Awake()
    {
        soundSource = GetComponent<AudioSource>();
    }

    /// <summary>
    /// Plays a sound effect once
    /// </summary>
    /// <param name="sound">Sound to play</param>
    /// <param name="volumeScale">Volume of the sound in %</param>
    public void Play(Sound sound, float volumeScale = 1) => soundSource.PlayOneShot(AudioClips.Get(sound), volumeScale);

    /// <summary>
    /// Starts looping a sound effect
    /// </summary>
    /// <param name="sound">Sound to play</param>
    /// <param name="volumeScale">Volume of the sound in %</param>
    public void PlayLoop(Sound sound, float volumeScale = 1)
    {
        if (soundLoops.TryGetValue(sound, out AudioSource audioSource))
        {
            audioSource.volume = volumeScale;
            audioSource.Play();
        }
        else
        {
            GameObject gameObject = new(sound.ToString() + " Loop");
            AudioSource newAudioSource = gameObject.AddComponent<AudioSource>();
            soundLoops.Add(sound, newAudioSource);
            newAudioSource.clip = AudioClips.Get(sound);
            newAudioSource.volume = volumeScale;
            newAudioSource.playOnAwake = false;
            newAudioSource.loop = true;
            newAudioSource.Play();
        }
    }
    /// <summary>
    /// Get the AudioSource used to play a looping sound effect
    /// </summary>
    /// <param name="sound">Sound the AudioSource plays</param>
    /// <returns>The audioSource if it exists, otherwise returns null</returns>
    public AudioSource GetAudioSource(Sound sound)
    {
        soundLoops.TryGetValue(sound, out AudioSource source);
        return source;
    }

    /// <summary>
    /// Stops looping a sound effect
    /// </summary>
    /// <param name="sound">Sound to stop playing</param>
    public void StopLoop(Sound sound)
    {
        AudioSource audioSource = GetAudioSource(sound);
        if (audioSource != null)
            audioSource.Stop();
    }
}
