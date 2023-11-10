using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Attack : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SendMessage("StopStaminaRegen");
        animator.SendMessage("DisableActions");
        animator.SendMessage("FreezeMovement");
        animator.SetBool("Attacking", true);
    }
    //public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    //{
    //    animator.SetBool("Attacking", false);
    //}
}
