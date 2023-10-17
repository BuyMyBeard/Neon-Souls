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
    public override void OnButtonNormal()
    {
        image.color = new Color(0, 0, 0, 0);
        transform.localScale = baseScale;
    }
    public override void OnButtonHighlighted()
    {
        transform.localScale = baseScale * scaleFactor;
        image.color = Color.white;
    }
    public override void OnButtonPressed()
    {
        transform.localScale = baseScale * clickedScaleFactor;
    }
}
