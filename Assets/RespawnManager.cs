using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class RespawnManager : MonoBehaviour
{
    public Vector3 respawnPosition { get; private set; }
    [SerializeField]CharacterController playerCharacter;


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
