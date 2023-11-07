using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitionBoundary : MonoBehaviour
{
    public float lowPassValue;
    [HideInInspector]
    public MusicTransitionManager transitionManager;
    // Start is called before the first frame update
    void Awake()
    {
        transitionManager = GetComponentInParent<MusicTransitionManager>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
