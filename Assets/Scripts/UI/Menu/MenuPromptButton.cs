using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuPromptButton : MaterialButton
{
    Image image;
    [SerializeField] float clickedScaleFactor = 0.8f;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }
    new public void OnButtonNormal()
    {
        image.color = new Color(0, 0, 0, 0);
        transform.localScale = baseScale;
    }
    new public void OnButtonHighlighted()
    {
        transform.localScale = baseScale * scaleFactor;
        image.color = Color.white;
    }
    new public void OnButtonPressed()
    {
        transform.localScale = baseScale * clickedScaleFactor;
    }
}
