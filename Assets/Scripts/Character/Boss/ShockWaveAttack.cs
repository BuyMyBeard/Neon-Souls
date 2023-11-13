using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShockWaveAttack : MonoBehaviour
{
    [SerializeField] Material shockWaveMaterial;
    [SerializeField] LayerMask player;
    [SerializeField] float baseDamage = 80;
    [SerializeField] float damageInflictedNormalizedTime = 0f;
    [SerializeField] float attackRadius = 10f;
    [SerializeField] float duration = 1f;
    [SerializeField] float initialStrength = -.7f;
    [SerializeField] float finalStrength = 0f;
    [SerializeField] float initialDistance = -0.1f;
    [SerializeField] float finalDistance = 2f;
    [SerializeField] float size = .05f;
    IEnumerator ShockWave()
    {
        bool hasDoneDamage = false;
        shockWaveMaterial.SetFloat("_Size", size);
        for (float t = 0; t < 1; t += Time.deltaTime / duration)
        {
            if (Time.timeScale == 0)
            {
                shockWaveMaterial.SetFloat("_ShockWaveStrength", 0);
                shockWaveMaterial.SetFloat("_WaveDistanceFromCenter", -.1f);
            }
            else
            {
                shockWaveMaterial.SetVector("_RingSpawnPosition", Camera.main.WorldToViewportPoint(transform.position));
                shockWaveMaterial.SetFloat("_ShockWaveStrength", Mathf.Lerp(initialStrength, finalStrength, t));
                shockWaveMaterial.SetFloat("_WaveDistanceFromCenter", Mathf.Lerp(initialDistance, finalDistance, t));
                if (!hasDoneDamage && t >= damageInflictedNormalizedTime)
                {
                    hasDoneDamage = true;
                    InflictDamage();
                }
            }
            yield return null;
        }
        ResetShockWaveMaterial();
    }

    [ContextMenu("Start Shockwave")]
    public void StartShockWave() => StartCoroutine(ShockWave());
    private void OnDestroy()
    {
        ResetShockWaveMaterial();
    }
    private void ResetShockWaveMaterial()
    {
        shockWaveMaterial.SetFloat("_ShockWaveStrength", 0);
        shockWaveMaterial.SetFloat("_WaveDistanceFromCenter", -.1f);
    }
    private void InflictDamage()
    {

    }
}
