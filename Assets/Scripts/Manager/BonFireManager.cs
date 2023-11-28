using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class BonfireManager : MonoBehaviour
{
    [SerializeField]Material activeMats;

    CharacterController playerCharacter;
    GameManager gameManager;
    Bonfire currentBonfire;

    public Vector3 RespawnPosition { get; private set; }

    private void Awake()
    {
        gameManager = FindObjectOfType<GameManager>();
        currentBonfire = GameObject.FindGameObjectWithTag("StartingBonfire").GetComponent<Bonfire>();
        playerCharacter = FindObjectOfType<CharacterController>();
    }
    private void Start()
    {
        //set up Game
        if(playerCharacter == null)
            throw new MissingComponentException("Character Controller component missing on character or Player tag is not set");

        // active first bonfire
        currentBonfire.active = true;

        //set spawnpoint at the first bonfire
        SetCurrentBonfire(currentBonfire);
    }
    public void ActivateBonfire(Bonfire bonefire) 
    {
        SetCurrentBonfire(bonefire);
    }
    public void SitAtBonfire(Bonfire bonefire)
    {
        // Ajouter Le Siting Animation et Tout autre behaviour quand le hero interagie avec le bonfire une fois activé
        SetCurrentBonfire(bonefire);
        gameManager.RechargeEverything(RechargeType.Rest);
    }
    private void SetCurrentBonfire(Bonfire bonfire)
    {
        currentBonfire = bonfire;
        RespawnPosition = bonfire.RespawnPosition;
    }
    public void Respawn()
    {
        playerCharacter.enabled = false;
        playerCharacter.transform.position = RespawnPosition;
        playerCharacter.enabled = true;
    }
}
