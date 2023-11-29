using System.Collections;
using UnityEngine;
using UnityEngine.UI;

public class FadeFilter : MonoBehaviour
{
    Image image;
    private void Awake()
    {
        image = GetComponent<Image>();
    }
    public void StartFadeOut(float time)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(time, 0));
    }

    public void StartFadeIn(float time)
    {
        StopAllCoroutines();
        StartCoroutine(Fade(time, 1));
    }

    IEnumerator Fade(float time, float fadeToAlpha)
    {
        Color startingColor = image.color;
        Color endingColor = startingColor;
        endingColor.a = fadeToAlpha;
        for (float t = 0; t < 1; t += Time.deltaTime / time)
        {
            image.color = Color.Lerp(startingColor, endingColor, t);
            yield return null;
        }
        image.color = endingColor;
    }
}
