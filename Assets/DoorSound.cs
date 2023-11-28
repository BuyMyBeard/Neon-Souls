using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(AudioSource))]
[RequireComponent(typeof(Rigidbody))]
public class DoorSound : MonoBehaviour
{
    [SerializeField] float pitchDeadzone = .1f;
    [SerializeField] float minPitch = .25f;
    [SerializeField] float maxPitch = 1f;
    [SerializeField] float pitchFactor = .01f;
    Rigidbody rb;
    AudioSource audioSource;
    
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        audioSource = GetComponent<AudioSource>();
    }

    private void Update()
    {
        float pitch = rb.angularVelocity­.magnitude / Time.deltaTime * pitchFactor;
        audioSource.pitch = pitch;
    }
}
