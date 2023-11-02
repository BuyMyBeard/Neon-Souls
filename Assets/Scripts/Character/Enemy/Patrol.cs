using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Patrol : MonoBehaviour
{
    [SerializeField] Task[] tasks;
    public struct Task
    {
        Transform patrolPoint;
        float waitTime;
    }
}
