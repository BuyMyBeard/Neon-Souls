using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerRootMotion : StateMachineBehaviour
{
    CharacterController characterController;
    PlayerMovement playerMovement;
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController = animator.GetComponent<CharacterController>();
        playerMovement = animator.GetComponentInParent<PlayerMovement>();
    }
    override public void OnStateMove(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        characterController.Move(animator.deltaPosition + Vector3.up * playerMovement.Gravity);
    }
}
