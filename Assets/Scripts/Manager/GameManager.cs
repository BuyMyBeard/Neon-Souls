using System.Collections;using System.Collections.Generic;using System.Linq;using System.Runtime.CompilerServices;using Unity.VisualScripting;using UnityEngine;public class GameManager : MonoBehaviour{    BonfireManager bonfireManager;
    GameObject player;    List<IRechargeable> rechargeables = new();
    Health health;
    ButtonPrompt buttonPrompt;
    CharacterController playerCharacter;
    PlayerMovement playerMovement;
    CameraMovement cameraMovement;
    private void Awake()    {
        player = GameObject.FindGameObjectWithTag("Player");        bonfireManager = GetComponent<BonfireManager>();        rechargeables = FindObjectsOfType<MonoBehaviour>().OfType<IRechargeable>().ToList();
        buttonPrompt = FindObjectOfType<ButtonPrompt>();
        health = player.GetComponentInParent<Health>();
        playerCharacter = player.GetComponentInParent<CharacterController>();
        playerMovement = player.GetComponentInParent<PlayerMovement>();
        cameraMovement = player.GetComponentInParent<CameraMovement>();    }    public void PlayerDie()    {        bonfireManager.Respawn();        RechargeEverything();    }    public void RechargeEverything()    {        // TODO: Enemy Spawns , Piege Recharge        foreach (IRechargeable rechargeable in rechargeables)        {            rechargeable.Recharge();        }    }
    public void StartIFrame() => health.invincible = true;
    public void StopIFrame() => health.invincible = false;
    public void FreezePlayer() { playerCharacter.enabled = false; playerMovement.frozen = true; }
    public void UnFreezePlayer() { playerCharacter.enabled = true; playerMovement.frozen = false; }
    public void FreezeCamera() => cameraMovement.enabled = false;
    public void UnFreezeCamera() => cameraMovement.enabled = true;}