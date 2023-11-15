using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;



[RequireComponent(typeof(VisualEffect))]
public class Explosion : MonoBehaviour
{
    [SerializeField]VisualEffect explosion;

    private void Awake()
    {
        explosion = GetComponent<VisualEffect>();
    }

    public void Explode()
    {
        Instantiate(explosion);
    }
}
