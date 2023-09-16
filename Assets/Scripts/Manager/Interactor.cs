using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interactor : MonoBehaviour
{

    [SerializeField] Transform handTransform;
    [SerializeField] float handRadius;
    [SerializeField] LayerMask layerInteration;
    readonly Collider[] colliders = new Collider[4];
    [SerializeField] int nbTrouvée;
    private void Update()
    {
        nbTrouvée = Physics.OverlapSphereNonAlloc(handTransform.position, handRadius, colliders, layerInteration); 
        foreach (Collider collider in colliders)
        {
            IInteractable interactable = collider.gameObject.GetComponent<IInteractable>();

            if(interactable != null)
            {

            }
        }
    }
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(handTransform.position, handRadius);
    }


}
