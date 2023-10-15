using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    Material buttonMat;
    [Range(0, 1)]
    [SerializeField] float thickness = 0.075f;
    [ColorUsage(true, true)]
    [SerializeField] Color color;
    [SerializeField] Color highlightedTextColor = Color.white;
    [SerializeField] float scaleFactor = 1.15f;
    [Range(1, 2)]
    [SerializeField] float clickedBoost = 1.3f;
    Vector3 baseScale;
    TextMeshProUGUI text;
    private void Awake()
    {
        buttonMat = new Material(Shader.Find("Shader Graphs/ButtonShader"));
        GetComponent<Image>().material = buttonMat;
        RectTransform rt = GetComponent<RectTransform>();
        buttonMat.SetFloat("_AspectRatio", rt.sizeDelta.x / rt.sizeDelta.y);
        buttonMat.SetFloat("_Thickness", thickness);
        buttonMat.SetColor("_Color", color);
        buttonMat.SetFloat("_ClickedBoost", clickedBoost);
        Debug.Log(rt.sizeDelta.x);
        Debug.Log(rt.sizeDelta.y);
        baseScale = transform.localScale;
        text = GetComponentInChildren<TextMeshProUGUI>();
    }
    private void OnEnable()
    {
        OnButtonNormal();
    }
    public void OnButtonNormal()
    {
        buttonMat.SetFloat("_Highlighted", 0);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale;
        text.color = color;
    }
    public void OnButtonHighlighted()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale * scaleFactor;
        text.color = highlightedTextColor;
    }
    public void OnButtonPressed()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 1);
    }
    public void OnButtonSelected()
    {
        OnButtonHighlighted();
    }
}
