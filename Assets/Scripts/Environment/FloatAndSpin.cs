using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloatAndSpin : MonoBehaviour
{
    [SerializeField] float spinSpeed = 1;
    [SerializeField] float amplitude = 1;
    [SerializeField] float frequency = 1;
    [SerializeField] float verticalShift = 1;
    float initialY;
    private void Start()
    {
        initialY = transform.position.y;
    }
    void Update()
    {
        transform.Rotate(0, spinSpeed, 0);
        transform.position = new Vector3(transform.position.x, initialY + verticalShift + Mathf.Sin(Time.time * frequency) * amplitude, transform.position.z);
    }
}
