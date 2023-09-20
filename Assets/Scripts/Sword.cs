using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(Collider))]
[RequireComponent(typeof(Rigidbody))]
public class Sword : MonoBehaviour
{
    [SerializeField] UnityEvent<Collider> onTrigger;
    new Collider collider;
    public bool ColliderEnabled
    {
        get => collider.enabled;
        set => collider.enabled = value;
    }
    void Awake()
    {
        collider = GetComponent<Collider>();
        collider.isTrigger = true;
        GetComponent<Rigidbody>().isKinematic = true;
    }
    void OnTriggerEnter(Collider other)
    {
        onTrigger.Invoke(other);
        Debug.Log("MOM! IT WORKED!");
    }
}