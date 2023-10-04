using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    [SerializeField] float travelSpeed = 5f;
    [SerializeField] float lifeSpan = 2f;
    [SerializeField] int bulletDamage = 60;
    Collider bulletCollider;
    Rigidbody rb;
    private void Awake()
    {
        bulletCollider = GetComponent<Collider>();
        rb = GetComponent<Rigidbody>();
    }
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) // 10 == Player layer
            collision.gameObject.GetComponentInParent<Health>().InflictDamage(bulletDamage);
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
