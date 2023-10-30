using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fireball : MonoBehaviour
{
    [SerializeField] Vector3 gravity;
    [SerializeField] int baseDamage = 100;
    public int damageScalingBonus = 0;
    Rigidbody rb;
    public bool thrown = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
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
        Destroy(gameObject);
    }
}
