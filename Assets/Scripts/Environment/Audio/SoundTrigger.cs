using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class SoundTrigger : MonoBehaviour
{
    AudioLoop musicManager;
    [SerializeField] AudioMixerSnapshot snapshot;
    [SerializeField] float time;
    // Start is called before the first frame update
    void Awake()
    {
        musicManager = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioLoop>();
    }

    private void OnTriggerEnter(Collider other)
    {
        snapshot.TransitionTo(time);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
