using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DeathScreen : MonoBehaviour
{
    [SerializeField] Color backgroundNotVisibleColor;
    [SerializeField] Color backgroundVisibleColor;
    [SerializeField] Color textNotVisibleColor;
    [SerializeField] Color textVisibleColor;
    [SerializeField] float fadeTime = 1.0f;
    Image background;
    TextMeshProUGUI text;

    private void Awake()
    {
        background = GetComponent<Image>();
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void Start()
    {
        background.color = backgroundNotVisibleColor;
        text.color = textNotVisibleColor;
    }

    public void SetVisible(bool visible)
    {
        if (visible)
        {
            background.color = backgroundVisibleColor;
            text.color = textVisibleColor;
        }
        else
        {
            background.color = backgroundNotVisibleColor;
            text.color = textNotVisibleColor;
        }
    }
    public void StartFadeIn()
    {
        StartCoroutine(FadeIn());
    }
    IEnumerator FadeIn()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / fadeTime)
        {
            background.color = Color.Lerp(backgroundNotVisibleColor, backgroundVisibleColor, t);
            text.color = Color.Lerp(textNotVisibleColor, textVisibleColor, t);
            yield return null;
        }
        background.color = backgroundVisibleColor;
        text.color = textVisibleColor;
    }

    // Dunno why I wrote this, I think I was high
    public void StartFadeOut()
    {
        StartCoroutine(FadeOut());
    }
    IEnumerator FadeOut()
    {
        for (float t = 0; t < 1; t += Time.deltaTime / fadeTime)
        {
            background.color = Color.Lerp(backgroundVisibleColor, backgroundNotVisibleColor, t);
            text.color = Color.Lerp(textVisibleColor, textNotVisibleColor, t);
            yield return null;
        }
        background.color = backgroundNotVisibleColor;
        text.color = textNotVisibleColor;
    }
    private void OnValidate()
    {
        GetComponent<Image>().color = backgroundVisibleColor;
        GetComponentInChildren<TextMeshProUGUI>().color = textVisibleColor;
    }
}
