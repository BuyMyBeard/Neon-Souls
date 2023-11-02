using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IEnemyBehaviour
{
    Coroutine actionInstance { get; set; }
    public IEnumerator Do();
}
