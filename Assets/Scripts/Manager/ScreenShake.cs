using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


[RequireComponent(typeof(CinemachineVirtualCamera))]
public class ScreenShake : MonoBehaviour
{
    CinemachineVirtualCamera vcam;
    CinemachineBasicMultiChannelPerlin noise;

    public static ScreenShake Instance { get; private set; }
    private void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(this);
        vcam = GetComponent<CinemachineVirtualCamera>();
        noise = vcam.GetCinemachineComponent<CinemachineBasicMultiChannelPerlin>();
    }
    public static void StartShake(AnimationCurve amplitude, AnimationCurve frequency, float duration = 1)
    {
        Instance.StopAllCoroutines();
        Instance.StartCoroutine(Instance.Shake(amplitude, frequency, duration));
    }
    public IEnumerator Shake(AnimationCurve amplitude, AnimationCurve frequency, float duration = 1)
    {
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            noise.m_AmplitudeGain = amplitude.Evaluate(t);
            noise.m_FrequencyGain = frequency.Evaluate(t);
            yield return null;
        }
        noise.m_AmplitudeGain = 0;
        noise.m_FrequencyGain = 0;
    }
}
