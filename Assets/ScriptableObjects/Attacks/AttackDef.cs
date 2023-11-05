using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackWeapon
{
    RightHandSword,
    LeftHandFist,
    LeftBackhand,
    GunHit
}
[CreateAssetMenu(fileName = "AttackDef", menuName = "Scriptables/Attack Def")]
public class AttackDef : ScriptableObject
{
    public AttackWeapon weapon;
    [Range(0, 200)]
    public int baseDamage;
    [Range(0, 100)]
    public int staminaCost;
}
