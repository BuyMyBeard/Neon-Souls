using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MusicTransitionManager : MonoBehaviour
{
    [Serializable]
    struct Checkpoint
    {
        public Transform transform;
        public float lowPassVal;
    }
    MusicManager musicManager;
    Transform player;
    [SerializeField] List<Checkpoint> checkpoints;
    void Awake()
    {
        player = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>().transform;
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<MusicManager>();
    }

    // Update is called once per frame
    void OnTriggerStay()
    {
        Debug.Log("in stay");
        int ckptId = -1;
        Checkpoint left, right;
        float progress = -1;
        do
        {
            ckptId++;
            left = checkpoints[ckptId];
            right = checkpoints[ckptId + 1 % checkpoints.Count];
            progress = Extensions.Vector3InverseLerp(left.transform.position, right.transform.position, player.position);
        } while (!progress.IsBetween(0, 1));
        float newLowPass = Mathf.Lerp(left.lowPassVal, right.lowPassVal, progress);
        musicManager.audioSource.outputAudioMixerGroup.audioMixer.SetFloat("LowPass", newLowPass);
    }
}
