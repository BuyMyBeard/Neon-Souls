using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrapBullet : MonoBehaviour
{
    [SerializeField] float travelSpeed = 2f;
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
        Destroy(gameObject);
    }
    public void MoveBullet(Vector3 direction)
    {
        rb.AddForce(direction * travelSpeed, ForceMode.VelocityChange);
    }
}
