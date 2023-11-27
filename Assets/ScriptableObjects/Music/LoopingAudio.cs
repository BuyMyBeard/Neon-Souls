using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "LoopingAudio", menuName = "Scriptables/Looping Audio")]
public class LoopingAudio : ScriptableObject
{
    public AudioClip audioClip;
    public int loopStartSample;
    public int loopEndSample;
}
