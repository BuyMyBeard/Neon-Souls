using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MaterialButton : MonoBehaviour
{
    Material buttonMat;
    [Range(0, 1)]
    [SerializeField] float thickness = 0.075f;
    [ColorUsage(false, true)]
    [SerializeField] Color color;
    [SerializeField] float scaleFactor = 1.15f;
    [Range(1, 2)]
    [SerializeField] float clickedBoost = 1.3f;
    Vector3 baseScale;
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
    }
    public void OnButtonNormal()
    {
        buttonMat.SetFloat("_Highlighted", 0);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale;

    }
    public void OnButtonHighlighted()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale * scaleFactor;
    }
    public void OnButtonPressed()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 1);
    }
    public void OnButtonSelected()
    {
        buttonMat.SetFloat("_Highlighted", 1);
        buttonMat.SetFloat("_Clicked", 0);
        transform.localScale = baseScale * scaleFactor;
    }
}
