using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    [SerializeField] float travelSpeed = 5f;
    public float lifeSpan = 1f;
    [SerializeField] int bulletDamage = 60;
    [SerializeField] int staminaBlockCost = 20;
    Rigidbody rb;
    bool hasAlreadyHit = false;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnTriggerEnter(Collider collider)
    {
        if (hasAlreadyHit) return;
        collider.gameObject.GetComponentInParent<Health>().InflictBlockableDamage(bulletDamage, staminaBlockCost, transform);
        Destroy(gameObject);
        hasAlreadyHit = true;
    }
    public void MoveBullet(Vector3 direction)
    {
        rb.AddForce(direction * travelSpeed, ForceMode.VelocityChange);
        StartCoroutine(TimeToDie());
    }
    IEnumerator TimeToDie()
    {
        yield return new WaitForSeconds(lifeSpan);
        Destroy(gameObject);
    }
}
