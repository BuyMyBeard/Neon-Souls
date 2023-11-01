using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class LifeSpan : MonoBehaviour
{
    float lifespan = 2f;

    public void OnEnable()
    {
        StartCoroutine(Lifespan());
    }
    IEnumerator Lifespan()
    {
        yield return new WaitForSeconds(lifespan);
        Destroy(gameObject);
    }
}
