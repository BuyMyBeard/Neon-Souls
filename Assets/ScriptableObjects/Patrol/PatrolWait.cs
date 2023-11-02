using System.Collections;
using System.Collections.Generic;
using UnityEngine;

enum Task { WalkToPosition, Wait }
public class PatrolWait : PatrolTask
{
    [SerializeField] float time;
}
