using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    protected Material buttonMat;
    [Range(0, 1)]
    [SerializeField] protected float thickness = 0.075f;
    [ColorUsage(true, true)]
    [SerializeField] protected Color color;
    [SerializeField] protected Color highlightedTextColor = Color.white;
    [SerializeField] protected float scaleFactor = 1.15f;
    [Range(1, 2)]
    [SerializeField] protected float clickedBoost = 1.3f;
    protected Vector3 baseScale;
    protected TextMeshProUGUI text;
    protected virtual void Awake()
    {
        buttonMat = new Material(Shader.Find("Shader Graphs/ButtonShader"));
        GetComponent<Image>().material = buttonMat;
        RectTransform rt = GetComponent<RectTransform>();
        buttonMat.SetFloat("_AspectRatio", rt.sizeDelta.x / rt.sizeDelta.y);
        buttonMat.SetFloat("_Thickness", thickness);
        buttonMat.SetColor("_Color", color);
        buttonMat.SetFloat("_ClickedBoost", clickedBoost);
        baseScale = transform.localScale;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    protected virtual void OnEnable()
    {
        OnButtonNormal();
    }
    public virtual void OnButtonNormal()
    {
        buttonMat.SetFloat("_Highlighted", 0);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale;
        text.color = color;
    }
    public virtual void OnButtonHighlighted()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale * scaleFactor;
        text.color = highlightedTextColor;
    }
    public virtual void OnButtonPressed()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 1);
    }
    public virtual void OnButtonSelected()
    {
        OnButtonHighlighted();
    }

}
