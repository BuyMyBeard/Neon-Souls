using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BonfireManager : MonoBehaviour
{
    public Vector3 respawnPosition { get; private set; }
    CharacterController playerCharacter;


    GameManager gameManager;
    //a enlever
    [SerializeField]Material activeMats;
    //

    Bonfire currentBonfire;
    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentBonfire = GameObject.FindGameObjectWithTag("StartingBonfire").GetComponent<Bonfire>();
        playerCharacter = GameObject.FindGameObjectWithTag("Player").GetComponent<CharacterController>();
    }
    
    private void Start()
    {
        //set up Game
        if(playerCharacter == null)
            throw new MissingComponentException("Character Controler component missing on character or Player tag is not set");

        // active first bonfire
        currentBonfire.Interact();

        //set spawnpoint at the first bonfire
        currentBonfire.Interact();
    }
    public void SetCurrentBonfire(Bonfire bonfire)
    {
        currentBonfire = bonfire;
        SetRepawn(bonfire.transform.position + bonfire.RespawnOffset);
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
    public void SetRepawn(Vector3 position)
    {
        respawnPosition = position;
    }

    public void Respawn()
    {
        playerCharacter.enabled = false;
        playerCharacter.transform.position = respawnPosition;
        playerCharacter.enabled = true;
    }
}
