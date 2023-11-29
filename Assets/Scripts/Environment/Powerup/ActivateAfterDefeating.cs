using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActivateAfterDefeating : MonoBehaviour
{
    [SerializeField] EnemyHealth enemyToDefeat;
    [SerializeField] float spawnDelay = 1.0f;

    private IEnumerator Start()
    {
        Renderer renderer = GetComponent<Renderer>();
        Collider collider = GetComponent<Collider>();
        renderer.enabled = false;
        collider.enabled = false;

        yield return new WaitUntil(() => enemyToDefeat.IsDead);

        yield return new WaitForSeconds(spawnDelay);
        renderer.enabled = true;
        collider.enabled = true;
    }
}
