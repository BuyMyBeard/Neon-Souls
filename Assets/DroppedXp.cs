using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DroppedXp : MonoBehaviour
{
    public int DroppedXpAmount;
    [SerializeField] PlayerExperience playerXp;
    private void Awake()
    {
        playerXp = FindObjectOfType<PlayerExperience>();
        DroppedXpAmount = playerXp.XpAmount;
        playerXp.removeXp(playerXp.XpAmount);
        
    }

    private void OnTriggerEnter(Collider other)
    {
        playerXp.GainXp(DroppedXpAmount);
        Destroy(this.gameObject);
    }
}
