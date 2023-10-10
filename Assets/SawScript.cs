using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    [SerializeField] float travelSpeed = 7f;
    [SerializeField] float travelDistance = 54f;
    [SerializeField] int sawDamage = 60;
    Vector3 initialPosition;
    Rigidbody rb;

    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
        initialPosition = transform.position;
    }

    public void MoveSaw()
    {
        rb.AddForce(transform.forward * travelSpeed, ForceMode.VelocityChange);
        StartCoroutine(SawCoroutine());
    }
    IEnumerator SawCoroutine()
    {
        yield return new WaitUntil(() => transform.position.x < initialPosition.x - travelDistance);
        Destroy(gameObject);
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.layer == 10) // 10 == Player layer
            collision.gameObject.GetComponentInParent<Health>().InflictDamage(sawDamage);
    }
}
