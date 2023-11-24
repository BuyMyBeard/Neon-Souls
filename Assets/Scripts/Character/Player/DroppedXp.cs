using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedXp : MonoBehaviour, IRechargeable
{
    public int DroppedXpAmount;
    [SerializeField] PlayerExperience playerXp;
    new Collider collider;
    private void Awake()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        DroppedXpAmount = playerXp.XpAmount;
        playerXp.RemoveXp(playerXp.XpAmount);
        collider = GetComponent<Collider>();
        GameManager.Instance.AddTemporaryRechargeable(this);
        collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        Debug.Log("player recovered Xp");
        playerXp.GainXp(DroppedXpAmount);
        Destroy(gameObject);
    }
    public void Recharge(RechargeType rechargeType)
    {
        collider.enabled = true;
    }
    void OnDestroy()
    {
        GameManager.Instance.RemoveTemporaryRechargeable(this);
    }
}
