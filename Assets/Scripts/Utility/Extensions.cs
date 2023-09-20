using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Extensions
{
    public static GameObject FindChildrenWithTag(this Transform parent, string tag)
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
                var result = FindChildrenWithTag(child, tag);
                if (result != null)
                    return result;
            }
        }
        return null;
    }
}
