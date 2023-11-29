using UnityEngine;

public class NightClubMusicResetter : MonoBehaviour, IRechargeable
{
    [SerializeField] GameObject music;
    AudioLowPassFilter lowPassFilter;
    AudioSource audioSource;
    [SerializeField] MusicTransitionCheckpoint startCheckpoint;
    // Start is called before the first frame update
    void Awake()
    {
        lowPassFilter = music.GetComponent<AudioLowPassFilter>();
        audioSource = music.GetComponent<AudioSource>();
        lowPassFilter.cutoffFrequency = startCheckpoint.lowPassValue;
        audioSource.volume = startCheckpoint.volume;
    }
    public void Recharge(RechargeType rechargeType = RechargeType.Respawn)
    {
        lowPassFilter.cutoffFrequency = startCheckpoint.lowPassValue;
        StartCoroutine(AudioFadeUtils.FadeIn(audioSource, 1, startCheckpoint.volume));
    }
}
