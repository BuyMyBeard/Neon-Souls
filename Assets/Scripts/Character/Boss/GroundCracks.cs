using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class GroundCracks : MonoBehaviour
{
    [SerializeField] float timeBeforeFade = 5;
    [SerializeField] float timeToFade = 1;
    IEnumerator Start()
    {
        DecalProjector projector = GetComponent<DecalProjector>();
        yield return new WaitForSeconds(timeBeforeFade);
        for (float t = 0; t < 1; t += Time.deltaTime / timeToFade)
        {
            projector.fadeFactor = 1 - t;
            yield return null;
        }
        Destroy(gameObject);
    }
}
