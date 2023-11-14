using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class MeteorAttackMark : MonoBehaviour
{
    [SerializeField] float growTime = 4;
    [SerializeField] float stayGrownTime = .5f;
    [SerializeField] float initialSize = .1f;
    [SerializeField] float finalSize = 1f;
    IEnumerator Start()
    {
        DecalProjector projector = GetComponent<DecalProjector>();
        for (float t = 0; t < 1; t += Time.deltaTime / growTime)
        {
            float size = Mathf.Lerp(initialSize, finalSize, t);
            projector.size = new Vector3(size, size, projector.size.z);
            yield return null;
        }
        yield return new WaitForSeconds(stayGrownTime);
        Destroy(gameObject);
    }
}
