using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AutoResetScroll : MonoBehaviour
{
    ScrollRect scrollRect;
    private void Awake()
    {
        scrollRect = GetComponent<ScrollRect>();
    }
    private void OnEnable()
    {
        scrollRect.verticalNormalizedPosition = 1;
    }
}
