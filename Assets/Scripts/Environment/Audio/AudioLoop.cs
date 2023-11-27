using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class AudioLoop : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;
    [SerializeField] LoopingAudio loopingAudio;
    public bool isLoopEnabled;
    [SerializeField] bool playOnAwake = false;
    [SerializeField] int startSampleDebug = 0;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = loopingAudio.audioClip;
        if (playOnAwake)
            audioSource.Play();
        if (loopingAudio.loopEndSample == 0)
            loopingAudio.loopEndSample = audioSource.clip.samples;
        audioSource.timeSamples = startSampleDebug;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoopEnabled && audioSource.timeSamples > loopingAudio.loopEndSample)
            audioSource.timeSamples = loopingAudio.loopStartSample;
    }
}
