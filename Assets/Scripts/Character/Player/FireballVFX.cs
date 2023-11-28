//using Microsoft.Unity.VisualStudio.Editor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

[RequireComponent(typeof(VisualEffect))]
public class FireballVFX : MonoBehaviour
{
    VisualEffect fireballVFX;
    float scale = 0;

    private void Awake()
    {
        fireballVFX = GetComponent<VisualEffect>();
    }

    private void OnEnable()
    {
        StartCoroutine(FiraballSpawning());
    }
    IEnumerator FiraballSpawning()
    {
        while (scale < 1) 
        {
            fireballVFX.SetFloat("sizeScale", scale += .1f);
            yield return new WaitForSeconds(0.1f);
        }

    }
}
