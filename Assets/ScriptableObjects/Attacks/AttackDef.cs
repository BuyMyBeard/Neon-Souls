using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum AttackWeapon
{
    RightHandSword,
    LeftHandFist,
    LeftBackhand,
    GunHit,
    FullBody,
    LeftFoot,
    MeteorSlam
}
[CreateAssetMenu(fileName = "AttackDef", menuName = "Scriptables/Attack Def")]
public class AttackDef : ScriptableObject
{
    public AttackWeapon weapon;
    public float baseDamageMultiplier;
    [Range(0, 100)]
    public int staminaCost;
    public bool blockable = true;
}
