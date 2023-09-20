using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BonfirerManager : MonoBehaviour
{
    RespawnManager respawnManager;


    //a enlever
    [SerializeField]Material spawningMats;
    [SerializeField]Material activeMats;
    //

    BoneFireInteractble spawningBonefire;
    private void Awake()
    {
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

        spawningBonefire = bonefire;


        // Ajouter Le Siting Animation et Tout autre behaviour quand le hero s'assie sur un bonfire
        respawnManager.SetRepawn(bonefire.transform.position);

        //

        spawningBonefire.GetComponent<Renderer>().material = spawningMats;
    }
}
