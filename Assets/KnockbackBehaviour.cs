using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class KnockbackBehaviour : StateMachineBehaviour
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
        characterController.Move(animator.deltaPosition * animator.GetFloat("Knockback") + Vector3.up * playerMovement.Gravity);
    }
}
