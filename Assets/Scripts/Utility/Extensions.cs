using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Extensions
{
    public static GameObject FindChildWithTag(this Transform parent, string tag)
    {
        for (int i = 0; i < parent.childCount; i++)
        {
            Transform child = parent.GetChild(i);
            if (child.CompareTag(tag))
            {
                return child.gameObject;
            }
            if (child.childCount > 0)
            {
                var result = FindChildWithTag(child, tag);
                if (result != null)
                    return result;
            }
        }
        return null;
    }
    public static float Vector3InverseLerp(Vector3 a, Vector3 b, Vector3 value)
    {
        Vector3 AB = b - a;
        Vector3 AV = value - a;
        return Vector3.Dot(AV, AB) / Vector3.Dot(AB, AB);
    }
    public static void ResetAllTriggers(this Animator animator)
    {
        foreach (var param in animator.parameters)
            if (param.type == AnimatorControllerParameterType.Trigger)
                animator.ResetTrigger(param.name);
    }
    public static void ResetAllBooleans(this Animator animator)
    {
        foreach (var param in animator.parameters)
            if (param.type == AnimatorControllerParameterType.Bool)
                animator.SetBool(param.name, false);
    }
    public static T PickRandom<T>(this IEnumerable<WeightedAction<T>> possibleActions)
    {
        float totalWeight = possibleActions.Sum(action => action.weight);
        List<(float, T)> chanceList = new();
        float current = 0;
        foreach (var action in possibleActions)
        {
            current += action.weight;
            chanceList.Add((current, action.actionName));
        }
        float pickedNumber = UnityEngine.Random.Range(0, totalWeight);
        return chanceList.Find(action => pickedNumber <= action.Item1).Item2;
    }
    public static bool IsBetween<T>(this T obj, T minBound, T maxBound) where T: IComparable
    {
        return obj.CompareTo(minBound) > -1 && obj.CompareTo(maxBound) < 1;
    }
}
[Serializable]
public struct WeightedAction<T>
{
    public WeightedAction(T actionName, float weight = 1)
    {
        this.actionName = actionName;
        this.weight = weight;
    }
    public T actionName;
    public float weight;
}
