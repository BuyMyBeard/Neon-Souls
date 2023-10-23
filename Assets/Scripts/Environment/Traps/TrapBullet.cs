using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    [SerializeField] float travelSpeed = 5f;
    [SerializeField] float lifeSpan = 1f;
    [SerializeField] int bulletDamage = 60;
    [SerializeField] int staminaBlockCost = 20;
    Rigidbody rb;
    private void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) // 10 == Player layer
            collision.gameObject.GetComponentInParent<PlayerHealth>().InflictBlockableDamage(bulletDamage, staminaBlockCost, transform);
        Destroy(gameObject);
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
