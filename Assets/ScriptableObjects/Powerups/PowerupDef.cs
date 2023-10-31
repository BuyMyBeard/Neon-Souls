using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PowerupBehaviour
{
    IncreaseAttack,
    MaxStamina,
    MaxHealth,
    MaxManas,
    MaxPotions
}
[CreateAssetMenu(fileName = "Powerup", menuName = "Scriptables/Powerup")]
public class PowerupDef : ScriptableObject
{   
    public PowerupBehaviour attributeToIncrease;
    public int amountToIncrease;
    public bool isTemporary;

    public void Apply()
    {
        //GameObject player = GameObject.FindGameObjectWithTag("Player");
        //switch (attributeToIncrease)
        //{
        //    case AttributeToIncrease.Attack:
        //        player.GetComponentInParent<MeleeAttack>().attackIncrease += amountToIncrease;
        //        break;

        //    default:
        //        break;
        //}
    }
}
