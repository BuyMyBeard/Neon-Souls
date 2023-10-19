using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonHighlightedState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SendMessage("OnButtonHighlighted");
    }
}
