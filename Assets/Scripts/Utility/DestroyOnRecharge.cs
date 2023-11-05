using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyOnRecharge : MonoBehaviour, IRechargeable
{
    private IEnumerator Start()
    {
        yield return null;
        GameManager.Instance.AddTemporaryRechargeable(this);
    }
    public void Recharge()
    {
        Destroy(gameObject);
    }

    public void OnDestroy()
    {
        GameManager.Instance.RemoveTemporaryRechargeable(this);
    }
}
