using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class BonefirerManager : MonoBehaviour
{
    [SerializeField]BoneFireInteratable startingBondFire;

    //a enlever
    [SerializeField]Material spawningMats;
    [SerializeField]Material activeMats;
    //

    List<BoneFireInteratable> boneFireInteratables = new();
    BoneFireInteratable spawningBonefire;
    private void Awake()
    {
        boneFireInteratables.AddRange(FindObjectsOfType<BoneFireInteratable>());

    }
    private void Start()
    {
        spawningBonefire = startingBondFire;
        spawningBonefire.Interact();
        spawningBonefire.Interact();
    }

    public void ActivateBonfire(BoneFireInteratable bonefire) 
    {
        //a remplacer par animation de feu ou effet
        bonefire.GetComponent<Renderer>().material = activeMats;
        //
    }

    public void SetSpawningBoneFire(BoneFireInteratable bonefire) 
    {
        spawningBonefire.GetComponent<Renderer>().material = activeMats;

        foreach (BoneFireInteratable bondFireInteratable in boneFireInteratables)
        {
            if (bondFireInteratable.gameObject == bonefire.gameObject)
            {
                spawningBonefire = bondFireInteratable;

                // Implement the spawning here instead
                spawningBonefire.GetComponent<Renderer>().material = spawningMats;
                Debug.Log(spawningBonefire.name);
                //
                break;
            }
        }
    }
}
