using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitionManager : MonoBehaviour
{
    MusicManager musicManager;
    [SerializeField] Transform player;
    [SerializeField] MusicTransitionBoundary leftBoundary;
    [SerializeField] MusicTransitionBoundary rightBoundary;
    void Awake()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    // Update is called once per frame
    void Update()
    {
        float progress = Extensions.Vector3InverseLerp(leftBoundary.transform.position, rightBoundary.transform.position, player.position);
        float newLowPass = Mathf.Lerp(leftBoundary.lowPassValue, rightBoundary.lowPassValue, progress);
        musicManager.audioSource.outputAudioMixerGroup.audioMixer.SetFloat("LowPass", newLowPass);
    }
}
