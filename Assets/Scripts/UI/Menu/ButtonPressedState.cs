using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonPressedState : StateMachineBehaviour
{
    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        animator.SendMessage("OnButtonPressed");
    }
}
