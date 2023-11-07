using System.Collections;
using UnityEngine;

public class PermanentPool : ObjectPool
{
    protected override IEnumerator ReturnCoroutine(GameObject obj)
    {
        yield return null;
    }
}