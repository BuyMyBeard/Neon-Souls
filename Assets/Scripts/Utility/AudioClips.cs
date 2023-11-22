using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public enum Sound
{
    Footstep1,
    Footstep2,
    Footstep3,
    Footstep4,
    Footstep5,
    Footstep6,
    Footstep7,
    Footstep8,
    Footstep9,
    Footstep10,
    PotionDrink,
    Kindle,
    Died,
    NakedRoll,
    LightRoll
}

public class AudioClips : MonoBehaviour
{
    void Awake()
    {
        clips.Clear();
        Instance = this;
        string[] audioclips = Enum.GetNames(typeof(Sound));
        Sound[] sounds = (Sound[])Enum.GetValues(typeof(Sound));
        for (int i = 0; i<sounds.Length; i++)
            clips.Add(sounds[i], Resources.Load<AudioClip>(audioclips[i]));
    }
    static readonly Dictionary<Sound, AudioClip> clips = new();
    static public AudioClip Get(Sound sound) => clips[sound];
    public static AudioClips Instance { get; private set; }
}
