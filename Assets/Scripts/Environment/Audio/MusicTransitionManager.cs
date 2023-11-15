using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitionManager : MonoBehaviour
{
    //[Serializable]
    //struct Checkpoint
    //{
    //    public Transform transform;
    //    public float lowPassVal;
    //}
    MusicManager musicManager;
    Transform player;
    //[SerializeField] List<Checkpoint> checkpoints;
    [SerializeField] bool isInTrigger = false;
    [SerializeField] MusicTransitionCheckpoint checkpoint1;
    [SerializeField] MusicTransitionCheckpoint checkpoint2;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().transform;
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    private void OnTriggerEnter(Collider other)
    {
        isInTrigger = true;
    }
    private void OnTriggerExit(Collider other)
    {
        isInTrigger = false;
    }

    private void Update()
    {
        if (isInTrigger)
        {
            float progress = Extensions.Vector3InverseLerp(checkpoint1.transform.position, checkpoint2.transform.position, player.position);
            Debug.Log($"Progress: {progress}");
            float newLowPass = Mathf.Lerp(checkpoint1.lowPassValue, checkpoint2.lowPassValue, progress);
            float newVolume = Mathf.Lerp(checkpoint1.volume, checkpoint2.volume, progress);
            musicManager.audioSource.GetComponent<AudioLowPassFilter>().cutoffFrequency = newLowPass;
            musicManager.audioSource.volume = newVolume;
        }
    }
}
