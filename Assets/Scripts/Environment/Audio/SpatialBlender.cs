using UnityEngine;
using Unity.Mathematics;

[RequireComponent(typeof(AudioSource))]
public class SpatialBlender : MonoBehaviour
{
    [SerializeField] AnimationCurve curve;
    [SerializeField] float minBlendingDistance;
    [SerializeField] float maxBlendingDistance;
    AudioListener audioListener;
    AudioSource audioSource;
    private void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioListener = FindObjectOfType<AudioListener>();
    }

    private void Update()
    {
        float normalizedDistance = Mathf.Clamp01(math.remap(minBlendingDistance, maxBlendingDistance, 0, 1, Vector3.Distance(audioListener.transform.position, transform.position)));
        audioSource.spatialBlend = curve.Evaluate(normalizedDistance);
    }
}