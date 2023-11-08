using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;

[RequireComponent(typeof(AudioSource))]
public class MusicManager : MonoBehaviour
{
    [HideInInspector]
    public AudioSource audioSource;
    public int loopStartSample;
    public int loopEndSample;
    public bool isLoopEnabled;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        if (loopEndSample == 0)
            loopEndSample = audioSource.clip.samples;
        audioSource.timeSamples = 12230778;
    }

    // Update is called once per frame
    void Update()
    {
        if (isLoopEnabled && audioSource.timeSamples > loopEndSample)
            audioSource.timeSamples = loopStartSample;
    }
}
