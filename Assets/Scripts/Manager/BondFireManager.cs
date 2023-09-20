using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BonefirerManager : MonoBehaviour
{
    RespawnManager respawnManager;


    //a enlever
    [SerializeField]Material spawningMats;
    [SerializeField]Material activeMats;
    //

    //List<BoneFireInteractble> boneFireInteratables = new();
    BoneFireInteractble spawningBonefire;
    private void Awake()
    {
       // boneFireInteratables.AddRange(FindObjectsOfType<BoneFireInteractble>());
        respawnManager = FindObjectOfType<RespawnManager>();
        spawningBonefire = GameObject.FindGameObjectWithTag("StartingBonfire").GetComponent<BoneFireInteractble>();
    }
    
    private void Start()
    {
        //set up Game

        // active first bonfire
        spawningBonefire.Interact();

        //set spawnpoint at the first bonfire
        spawningBonefire.Interact(); 
    }

    public void ActivateBonfire(BoneFireInteractble bonefire) 
    {
        //a remplacer par animation de feu ou effet
        bonefire.GetComponent<Renderer>().material = activeMats;
        //
    }

    public void SetSpawningBoneFire(BoneFireInteractble bonefire) 
    {
        // a enlever
        spawningBonefire.GetComponent<Renderer>().material = activeMats;
        //
        
        respawnManager.SetRepawn(bonefire.transform.position);
        spawningBonefire = bonefire;
        spawningBonefire.GetComponent<Renderer>().material = spawningMats;

        /*
        foreach (BoneFireInteractble bonFireInteratable in boneFireInteratables)
        {
            if (bonFireInteratable.gameObject == bonefire.gameObject)
            {
                spawningBonefire = bonFireInteratable;
                //a enlever
                spawningBonefire.GetComponent<Renderer>().material = spawningMats;
                //
                break;
            }
        }*/

    }
}
