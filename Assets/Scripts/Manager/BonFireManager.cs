using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BonfirerManager : MonoBehaviour
{
    RespawnManager respawnManager;
    GameManager gameManager;
    //a enlever
    [SerializeField]Material spawningMats;
    [SerializeField]Material activeMats;
    //

    BoneFireInteractble spawningBonefire;
    private void Awake()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
        gameManager = FindObjectOfType<GameManager>();
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
    public void SetSpawningBonFire(BoneFireInteractble bonefire)
    {
        spawningBonefire = bonefire;
        respawnManager.SetRepawn(bonefire.transform.position);
    }

    public void ActivateBonfire(BoneFireInteractble bonefire) 
    {
        SetSpawningBonFire(bonefire);
        //a remplacer par animation de feu ou effet
        bonefire.GetComponent<Renderer>().material = activeMats;
        //
    }
    public void SitAtBonFire(BoneFireInteractble bonefire)
    {
        // Ajouter Le Siting Animation et Tout autre behaviour quand le hero inteeragie avec le bonfire une fois activé
        SetSpawningBonFire(bonefire);
        gameManager.RechargeEverything();
    }
}
