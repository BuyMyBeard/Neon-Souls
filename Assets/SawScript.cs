using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SawScript : MonoBehaviour
{
    [SerializeField] float travelSpeed = 5f;
    [SerializeField] int sawDamage = 60;

    Rigidbody rb;
    // Start is called before the first frame update
    void Awake()
    {
        rb = GetComponent<Rigidbody>();
    }

    public void MoveSaw()
    {
        rb.AddForce(transform.forward * travelSpeed, ForceMode.VelocityChange);
    }
}
