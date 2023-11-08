using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PhysicsWihtotherObject : MonoBehaviour
{
    [SerializeField] float forceDePousser = 2; 
    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        Rigidbody rbObject = hit.collider.attachedRigidbody;
        if (rbObject == null || rbObject.isKinematic)
            return;
        if (hit.moveDirection.y < -0.3f)
            return;

        Vector3 pushDirection = new Vector3(hit.moveDirection.x,0,hit.moveDirection.z);

        rbObject.velocity = pushDirection * forceDePousser;


    }
}
