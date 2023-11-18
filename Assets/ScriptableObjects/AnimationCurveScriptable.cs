using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "AnimationCurveScriptable", menuName = "Scriptables/Animation Curve")]
public class AnimationCurveScriptable : ScriptableObject
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float length;
    public float Evaluate(float time) => curve.Evaluate(time / length);
}
