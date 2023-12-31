using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSoundDef", menuName = "Scriptables/RandomSoundDef")]
public class RandomSoundDef : ScriptableObject
{
    public WeightedAction<Sound>[] soundPool;
    public float volume = 1;
}
