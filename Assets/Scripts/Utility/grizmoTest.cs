using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class grizmoTest : MonoBehaviour
{
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawRay(transform.position,transform.forward);
        Gizmos.DrawWireCube(transform.position,new Vector3(1,0,1));
    }
}
