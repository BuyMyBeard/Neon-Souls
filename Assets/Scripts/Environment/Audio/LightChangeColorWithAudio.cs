using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
public class LightChangeColorWithAudio : MonoBehaviour
{
    AudioSource audioSource;
    [SerializeField] List<Light> spotlights;
    [SerializeField] List<Color> colors;
    [SerializeField] int spectrumBandsToSkip;
    [SerializeField] int spectrumBandsToTake;
    [SerializeField] float avgHeight;
    [SerializeField] float threshold;
    // Start is called before the first frame update
    void Awake()
    {
        audioSource = GameObject.FindGameObjectWithTag("MusicManager").GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        float[] spectrum = new float[1024];
        audioSource.GetSpectrumData(spectrum, 0, FFTWindow.Rectangular);
        float newAvgHeight = spectrum.Skip(spectrumBandsToSkip).Take(spectrumBandsToTake).Average() * 1000;
        if (avgHeight > threshold && newAvgHeight <= threshold)
        {
            Color newColor;
            do
            {
                newColor = colors[Random.Range(0, colors.Count)];
            } while (spotlights[0].color == newColor);

            foreach (var spotlight in spotlights)
                spotlight.color = newColor;
        }
        avgHeight = newAvgHeight;
    }
}
