using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Audio;

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
    /// Pick a random sound according to weight to play once
    /// </summary>
    /// <param name="soundPool"></param>
    /// <param name="volumeScale"></param>
    public void PlayRandom(WeightedAction<Sound>[] soundPool, float volumeScale = 1) => soundSource.PlayOneShot(AudioClips.Get(soundPool.PickRandom()), volumeScale);
    /// <summary>
    /// Pick a random sound, all weighted the same
    /// </summary>
    /// <param name="sounds"></param>
    /// <param name="volumeScale"></param>
    public void PlayRandom(IEnumerable<Sound> sounds, float volumeScale = 1) => soundSource.PlayOneShot(AudioClips.Get(sounds.ElementAt(Random.Range(0, sounds.Count() - 1))), volumeScale);
    public void PlayRandom(RandomSoundDef sounds) => soundSource.PlayOneShot(AudioClips.Get(sounds.soundPool.PickRandom()), sounds.volume);

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
            newAudioSource.outputAudioMixerGroup = soundSource.outputAudioMixerGroup;
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
