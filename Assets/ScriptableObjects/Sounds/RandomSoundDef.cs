using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "RandomSoundDef", menuName = "Scriptables/RandomSoundDef")]

public class RandomSoundDef : MonoBehaviour
{
    public WeightedAction<Sound>[] soundPool;
    public float volume = 1;
}
