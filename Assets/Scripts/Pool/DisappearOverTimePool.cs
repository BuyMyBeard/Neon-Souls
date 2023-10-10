using System.Collections;
using UnityEngine;

public class DisappearOverTimePool : ObjectPool
{
    [SerializeField] float cooldown;
    protected override IEnumerator ReturnCoroutine(GameObject obj)
    {
        yield return new WaitForSeconds(cooldown);
        ReturnObject(obj);
    }
}