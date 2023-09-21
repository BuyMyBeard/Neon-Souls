using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    LockOn lockOn;
    // Start is called before the first frame update
    private void Awake()
    {
        lockOn = FindObjectOfType<LockOn>();
    }
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnBecameVisible()
    {
        lockOn.AddToLockOnList(this);
    }
    private void OnBecameInvisible()
    {
        lockOn.RemoveFromLockOnList(this);
    }
}
