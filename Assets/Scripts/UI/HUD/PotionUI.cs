using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PotionUI : MonoBehaviour
{
    [SerializeField] Material potionMat;

    private void Start()
    {
        FillLevel = 0.5f;
    }
    public float FillLevel
    {
        get => potionMat.GetFloat("_Height");
        set => potionMat.SetFloat("_Height", value);
    }
}
