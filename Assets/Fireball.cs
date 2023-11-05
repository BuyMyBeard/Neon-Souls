using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;


public class Fireball : MonoBehaviour
{
    [SerializeField] Vector3 gravity;
    [SerializeField] int baseDamage = 100;
    [SerializeField] GameObject explosionPrefab;
    public int damageScalingBonus = 0;
    Rigidbody rb;
    Explosion explosion;
    public bool thrown = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
        explosion = GetComponent<Explosion>();
    }
    private void FixedUpdate()
    {
        if (thrown)
            rb.AddForce(gravity, ForceMode.Acceleration);
    }
    private void Update()
    {
        transform.rotation = Quaternion.identity;
    }
    private void OnCollisionEnter(Collision collision)
    {
        Health opponentHealth = collision.gameObject.GetComponentInParent<Health>();
        if (opponentHealth != null)
            opponentHealth.InflictDamage(baseDamage + damageScalingBonus);

        // TODO: Particle explosion
        Instantiate(explosionPrefab,transform.position,Quaternion.identity);
        Destroy(gameObject);
    }
}
