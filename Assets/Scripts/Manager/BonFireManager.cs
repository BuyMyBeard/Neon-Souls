using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BonfireManager : MonoBehaviour
{
    RespawnManager respawnManager;
    GameManager gameManager;
    //a enlever
    [SerializeField]Material spawningMats;
    [SerializeField]Material activeMats;
    //

    Bonfire currentBonfire;
    private void Awake()
    {
        respawnManager = FindObjectOfType<RespawnManager>();
        gameManager = FindObjectOfType<GameManager>();
        currentBonfire = GameObject.FindGameObjectWithTag("StartingBonfire").GetComponent<Bonfire>();
    }
    
    private void Start()
    {
        //set up Game

        // active first bonfire
        currentBonfire.Interact();

        //set spawnpoint at the first bonfire
        currentBonfire.Interact();
    }
    public void SetCurrentBonfire(Bonfire bonfire)
    {
        currentBonfire = bonfire;
        respawnManager.SetRepawn(bonfire.transform.position + bonfire.RespawnOffset);
    }

    public void ActivateBonfire(Bonfire bonefire) 
    {
        SetCurrentBonfire(bonefire);
        //a remplacer par animation de feu ou effet
        bonefire.GetComponent<Renderer>().material = activeMats;
        //
    }
    public void SitAtBonfire(Bonfire bonefire)
    {
        // Ajouter Le Siting Animation et Tout autre behaviour quand le hero inteeragie avec le bonfire une fois activé
        SetCurrentBonfire(bonefire);
        gameManager.RechargeEverything();
    }
}
