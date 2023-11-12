using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "SoundDef", menuName = "Scriptables/SoundDef")]
public class SoundDef : ScriptableObject
{
    public Sound sound;
    public float volume = 1;
}
