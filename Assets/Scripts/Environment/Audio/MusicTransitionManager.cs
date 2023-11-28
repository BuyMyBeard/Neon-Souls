using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitionManager : MonoBehaviour, IRechargeable
{
    //[Serializable]
    //struct Checkpoint
    //{
    //    public Transform transform;
    //    public float lowPassVal;
    //}
    AudioLoop audioLoop;
    Transform player;
    //[SerializeField] List<Checkpoint> checkpoints;
    [SerializeField] bool isInTrigger = false;
    [SerializeField] MusicTransitionCheckpoint checkpoint1;
    [SerializeField] MusicTransitionCheckpoint checkpoint2;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponentInChildren<CharacterController>().transform;
        audioLoop = transform.parent.GetComponentInChildren<AudioLoop>();
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
            float newLowPass = Mathf.Lerp(checkpoint1.lowPassValue, checkpoint2.lowPassValue, progress);
            float newVolume = Mathf.Lerp(checkpoint1.volume, checkpoint2.volume, progress);
            audioLoop.audioSource.GetComponent<AudioLowPassFilter>().cutoffFrequency = newLowPass;
            audioLoop.audioSource.volume = newVolume;
        }
    }
    public void Recharge(RechargeType rechargeType)
    {
        isInTrigger = false;
    }
}
